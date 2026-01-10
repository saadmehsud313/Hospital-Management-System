Query For creating doctor view  :

Create view DoctorView as

select st.StaffID as StaffNo,doc.DoctorID as DocID,
st.FirstName as [F.Name],
st.LastName as [L.Name],
st.Role,Doc.Speciality,doc.MaxAppointment,Doc.ConsultationFee from staff st 
inner join doctor doc
on st.StaffID=doc.StaffID and st.Role='Doctor'

--Procedure query to fetch appointment data on the basis of doctor id ->
create procedure GetAppointmentByDocID @id int
as
begin
	select * from Appointment
where DoctorID in (
select DoctorID from Doctor where StaffID=@id)
end

--Unique Key Constraint For MRN 
ALTER TABLE Patient
ADD CONSTRAINT UQ_Patient_MRN UNIQUE (MRN);


--Store Procedure for Fetching appointments that are today->
create procedure GetTodayAppointmentByDocID @DoctorId int 
as 
begin
  SELECT 
                            a.AppointmentID,
                            a.PatientID,
                            a.DoctorID,
                            a.CreatedByStaffID,
                            a.ScheduledAt,
                            a.Status,
                            a.Reason,
                            ISNULL(p.FirstName + ' ' + p.LastName, 'Unknown Patient') as PatientName,
                            ISNULL(p.Phone, 'N/A') as PatientPhone
                        FROM Appointment a
                        LEFT JOIN Patient p ON a.PatientID = p.PatientID
                        WHERE a.DoctorID = @DoctorID
                        AND CAST(a.ScheduledAt AS DATE) = CAST(GETDATE() AS DATE)
                        AND a.Status IN ('Scheduled', 'Pending', 'ScheduledToday', 'Attended')
                        ORDER BY a.ScheduledAt ASC
end
-- Store Procedure to get pending appointments with doctorId ->
create Procedure GetPendingAppointments @DoctorID int
as 
begin 
 SELECT 
                            a.AppointmentID,
                            a.PatientID,
                            a.DoctorID,
                            a.CreatedByStaffID,
                            a.ScheduledAt,
                            a.Status,
                            a.Reason,
                            ISNULL(p.FirstName + ' ' + p.LastName, 'Unknown Patient') as PatientName,
                            ISNULL(p.Phone, 'N/A') as PatientPhone
                        FROM Appointment a
                        LEFT JOIN Patient p ON a.PatientID = p.PatientID
                        WHERE a.DoctorID = @DoctorID
                        AND CAST(a.ScheduledAt AS DATE) > CAST(GETDATE() AS DATE)
                        AND a.Status IN ('Scheduled', 'Pending')
                        ORDER BY a.ScheduledAt ASC
end









create Procedure GetPendingAppointmentsByDocID @DoctorID int
as
begin
SELECT 
                            a.AppointmentID,
                            a.PatientID,
                            a.DoctorID,
                            a.CreatedByStaffID,
                            a.ScheduledAt,
                            a.Status,
                            a.Reason,
                            ISNULL(p.FirstName + ' ' + p.LastName, 'Unknown Patient') as PatientName,
                            ISNULL(p.Phone, 'N/A') as PatientPhone
                        FROM Appointment a
                        LEFT JOIN Patient p ON a.PatientID = p.PatientID
                        WHERE a.DoctorID = @DoctorID
                        AND (
                            CAST(a.ScheduledAt AS DATE) < CAST(GETDATE() AS DATE)
                            OR a.Status IN ('Completed', 'Cancelled', 'Attended', 'Not Attended')
                        )
                        ORDER BY a.ScheduledAt DESC


                        end


--Stored Procedure For Insert Patient 
Create PROCEDURE InsertPatient
(
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @Gender VARCHAR(50),
    @Phone VARCHAR(50),
    @BloodGroup VARCHAR(50),
    @EmergencyContactName VARCHAR(50),
    @EmergencyContactPhone VARCHAR(50),
    @CreatedAt DATETIME,
    @IsActive BIT,
    @Age INT
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @PatientID INT;
    DECLARE @MRN VARCHAR(50);
    DECLARE @CreatedAtStr VARCHAR(10);

    --  Generate PatientID automatically
    SELECT @PatientID = ISNULL(MAX(PatientID), 0) + 1
    FROM Patient;

    --  Format CreatedAt for MRN (MMMYY)
    SET @CreatedAtStr = UPPER(FORMAT(@CreatedAt, 'MMMyy'));

    -- Generate MRN
    SET @MRN = @CreatedAtStr + '-' + CAST(@PatientID AS VARCHAR(10));

    -- Insert patient
    INSERT INTO Patient
    (
        PatientID,
        MRN,
        FirstName,
        LastName,
        Gender,
        Phone,
        BloodGroup,
        EmergencyContactName,
        EmergencyContactPhone,
        CreatedAt,
        IsActive,
        Age
    )
    VALUES
    (
        @PatientID,
        @MRN,
        @FirstName,
        @LastName,
        @Gender,
        @Phone,
        @BloodGroup,
        @EmergencyContactName,
        @EmergencyContactPhone,
        @CreatedAt,
        @IsActive,
        @Age
    );

    --Return success message and generated IDs
    SELECT 1 AS Success,
           'Patient inserted successfully.' AS Message,
           @PatientID AS PatientID,
           @MRN AS MRN;
END;



--Stored Procedure For  Inserting Appointment

CREATE PROCEDURE sp_InsertAppointment
(
    @PatientID INT,
    @DoctorID INT,
    @CreatedByStaffID INT,
    @ScheduledAt DATETIME,
    @Reason VARCHAR(255),
    @Status VARCHAR(50)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @LastAppointment DATETIME;
    DECLARE @AppointmentCount INT;
    DECLARE @MaxAppointments INT;
    DECLARE @NewScheduledAt DATETIME;
    DECLARE @AppointmentID INT;

    -- Generate new AppointmentID
    SELECT @AppointmentID = ISNULL(MAX(AppointmentID), 0) + 1
    FROM Appointment;

    -- Get last appointment for doctor that day
    SELECT @LastAppointment = MAX(ScheduledAt)
    FROM Appointment
    WHERE DoctorID = @DoctorID
      AND CAST(ScheduledAt AS DATE) = CAST(@ScheduledAt AS DATE);

    -- Count appointments for doctor that day
    SELECT @AppointmentCount = COUNT(*)
    FROM Appointment
    WHERE DoctorID = @DoctorID
      AND CAST(ScheduledAt AS DATE) = CAST(@ScheduledAt AS DATE);

    -- Get max appointments per doctor
    SELECT @MaxAppointments = MaxAppointment
    FROM Doctor
    WHERE DoctorID = @DoctorID;

   -- Check max appointments limit
    IF @AppointmentCount >= @MaxAppointments
    BEGIN
        -- Return error to .NET MAUI as a result set
        SELECT 0 AS Success, 
               'Maximum appointments reached for this doctor on the selected day.' AS Message;
        RETURN;
    END

    --  Compute scheduled time
    IF @LastAppointment IS NOT NULL AND @LastAppointment >= @ScheduledAt
    BEGIN
        SET @NewScheduledAt = DATEADD(MINUTE, 30, @LastAppointment);
    END
    ELSE
    BEGIN
        SET @NewScheduledAt = @ScheduledAt;
    END

    -- Insert appointment
    INSERT INTO Appointment
    (
        AppointmentID,
        PatientID,
        DoctorID,
        CreatedByStaffID,
        ScheduledAt,
        Reason,
        Status
    )
    VALUES
    (
        @AppointmentID,
        @PatientID,
        @DoctorID,
        @CreatedByStaffID,
        @NewScheduledAt,
        @Reason,
        @Status
    );

    --  Return success info to .NET MAUI
    SELECT 1 AS Success, 
           'Appointment scheduled successfully.' AS Message,
           @AppointmentID AS AppointmentID,
           @NewScheduledAt AS ScheduledAt;

END;

--Stored Procedure For Updating Appointment 
CREATE PROCEDURE sp_UpdateAppointment
(
    @AppointmentID INT,
    @PatientID INT,
    @DoctorID INT,
    @CreatedByStaffID INT,
    @ScheduledAt DATETIME,
    @Reason VARCHAR(255),
    @Status VARCHAR(50)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @LastAppointment DATETIME;
    DECLARE @AppointmentCount INT;
    DECLARE @MaxAppointments INT;
    DECLARE @NewScheduledAt DATETIME;


    -- Get last appointment for doctor on the same day, excluding this appointment
    SELECT @LastAppointment = MAX(ScheduledAt)
    FROM Appointment
    WHERE DoctorID = @DoctorID
      AND CAST(ScheduledAt AS DATE) = CAST(@ScheduledAt AS DATE)
      AND AppointmentID <> @AppointmentID;

    --  Count appointments for doctor that day, excluding this appointment
    SELECT @AppointmentCount = COUNT(*)
    FROM Appointment
    WHERE DoctorID = @DoctorID
      AND CAST(ScheduledAt AS DATE) = CAST(@ScheduledAt AS DATE)
      AND AppointmentID <> @AppointmentID;

    -- Get doctor's max appointments
    SELECT @MaxAppointments = MaxAppointment
    FROM Doctor
    WHERE DoctorID = @DoctorID;

    --  Check max appointments
    IF @AppointmentCount >= @MaxAppointments
    BEGIN
        SELECT 0 AS Success,
               'Maximum appointments reached for this doctor on the selected day.' AS Message,
               NULL AS AppointmentID,
               NULL AS ScheduledAt;
        RETURN;
    END

    --  Compute new scheduled time if conflict exists
    IF @LastAppointment IS NOT NULL AND @LastAppointment >= @ScheduledAt
        SET @NewScheduledAt = DATEADD(MINUTE, 30, @LastAppointment);
    ELSE
        SET @NewScheduledAt = @ScheduledAt;

    --  Update appointment
    UPDATE Appointment
    SET PatientID = @PatientID,
        DoctorID = @DoctorID,
        CreatedByStaffID = @CreatedByStaffID,
        ScheduledAt = @NewScheduledAt,
        Reason = @Reason,
        Status = @Status
    WHERE AppointmentID = @AppointmentID;

    --  Return success message
    SELECT 1 AS Success,
           'Appointment updated successfully.' AS Message,
           @AppointmentID AS AppointmentID,
           @NewScheduledAt AS ScheduledAt;
END;




--Create visit Table Query
CREATE TABLE Visit
(
    VisitID INT NOT NULL,
    DoctorID INT NOT NULL,
    PatientID INT NOT NULL,
    AppointmentID INT NOT NULL,

    DiagnosisSummary VARCHAR(500) NOT NULL,
    Prescriptions VARCHAR(500) NOT NULL,
    Symptoms VARCHAR(500) NOT NULL,
    VisitType VARCHAR(50) NOT NULL,

    VisitDateTime DATETIME NOT NULL,
    CreatedAt DATETIME NOT NULL,
    FollowUpDate DATETIME NULL,

    -- Primary Key
    CONSTRAINT PK_Visit PRIMARY KEY (VisitID),

    -- Foreign Keys
    CONSTRAINT FK_Visit_Doctor
        FOREIGN KEY (DoctorID)
        REFERENCES Doctor(DoctorID),

    CONSTRAINT FK_Visit_Patient
        FOREIGN KEY (PatientID)
        REFERENCES Patient(PatientID),

    CONSTRAINT FK_Visit_Appointment
        FOREIGN KEY (AppointmentID)
        REFERENCES Appointment(AppointmentID)
);
GO
--Insert Visit Procedure and trigger for updating appointment status to 1
CREATE PROCEDURE sp_InsertVisit
(
    @DoctorID INT,
    @PatientID INT,
    @AppointmentID INT,
    @DiagnosisSummary VARCHAR(500),
    @Prescriptions VARCHAR(500),
    @Symptoms VARCHAR(500),
    @VisitType VARCHAR(50),
    @VisitDateTime DATETIME,
    @CreatedAt DATETIME,
    @FollowUpDate DATETIME = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @VisitID INT;

    --  Auto-generate VisitID
    SELECT @VisitID = ISNULL(MAX(VisitID), 6000) + 1
    FROM Visit;

    --  Insert Visit
    INSERT INTO Visit
    (
        VisitID,
        DoctorID,
        PatientID,
        AppointmentID,
        DiagnosisSummary,
        Prescriptions,
        Symptoms,
        VisitType,
        VisitDateTime,
        CreatedAt,
        FollowUpDate
    )
    VALUES
    (
        @VisitID,
        @DoctorID,
        @PatientID,
        @AppointmentID,
        @DiagnosisSummary,
        @Prescriptions,
        @Symptoms,
        @VisitType,
        @VisitDateTime,
        @CreatedAt,
        @FollowUpDate
    );

    
    SELECT 1 AS Success,
           'Visit created successfully.' AS Message,
           @VisitID AS VisitID;
END;

--Visit Update Trigger
CREATE TRIGGER trg_Visit_UpdateAppointmentStatus
ON Visit
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE A
    SET A.Status = 'Completed'
    FROM Appointment A
    INNER JOIN inserted I
        ON A.AppointmentID = I.AppointmentID;
END;

--Create Admit Request Store Procedure
CREATE PROCEDURE sp_CreateAdmitRequest
(
    @PatientID INT,
    @DoctorID INT
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @RequestID INT;

    

 
--       Generate RequestID
    SELECT @RequestID = ISNULL(MAX(RequestID), 7000) + 1
    FROM Admit_Request;

--       Insert Admit Request
    INSERT INTO Admit_Request
    (
        RequestID,
        PatientID,
        DocID,
        RespondedByStaffID,
        RequestStatus
    )
    VALUES
    (
        @RequestID,
        @PatientID,
        @DoctorID,
        NULL,
        'Pending'
    );

--       Success Response
    SELECT 1 AS Success,
           'Admit request created successfully.' AS Message,
           @RequestID AS RequestID;
END;


--Update Admit Request
CREATE PROCEDURE sp_UpdateAdmitRequest
(
    @RequestID INT,
    @RespondedByStaffID INT,
    @Status VARCHAR(20)   
)
AS
BEGIN
    SET NOCOUNT ON;

   
    --    Prevent double update
      IF EXISTS
    (
        SELECT 1
        FROM Admit_Request
        WHERE RequestID = @RequestID
          AND RequestStatus <> 'Pending'
    )
    BEGIN
        SELECT 0 AS Success,
               'Admit request already processed.' AS Message;
        RETURN;
    END
    --   Update Request
      UPDATE Admit_Request
    SET
        RequestStatus = @Status,
        RespondedByStaffID = @RespondedByStaffID
    WHERE RequestID = @RequestID;

    --Success Response
      SELECT 1 AS Success,
           'Admit request updated successfully.' AS Message;
END;

-- Fetch Room_assignment
CREATE OR ALTER PROCEDURE GetRoomAssignments
(
    @NurseID INT = NULL,
    @RoomID INT = NULL,
    @IsActive BIT = NULL,
    @FromDate DATE = NULL,
    @ToDate DATE = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ar.AssignmentID,
        ar.NurseID,
        ar.RoomID,
        ar.AssignmentDate,
        ar.AssignedByStaffID,
        ar.IsActive,
        ar.Shift
    FROM Assignment_Room ar
    WHERE
        (@NurseID IS NULL OR ar.NurseID = @NurseID)
        AND (@RoomID IS NULL OR ar.RoomID = @RoomID)
        AND (@IsActive IS NULL OR ar.IsActive = @IsActive)
        AND (@FromDate IS NULL OR ar.AssignmentDate >= @FromDate)
        AND (@ToDate IS NULL OR ar.AssignmentDate <= @ToDate)
    ORDER BY ar.AssignmentDate DESC;
END;
