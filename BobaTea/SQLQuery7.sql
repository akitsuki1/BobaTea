CREATE TABLE [dbo].[JobApplications] (
    [ApplicationId]     INT IDENTITY(1,1) PRIMARY KEY,       -- Khóa chính
    [FullName]          NVARCHAR(100) NOT NULL,              -- Họ và tên
    [Email]             NVARCHAR(100) NOT NULL,              -- Email liên hệ
    [Phone]             NVARCHAR(15)  NOT NULL,              -- Số điện thoại
    [Position]          NVARCHAR(50)  NOT NULL,              -- Vị trí tuyển dụng
    [Introduction]      NVARCHAR(MAX) NULL,                  -- Giới thiệu bản thân
    [CvFilePath]        NVARCHAR(255) NULL,                  -- Đường dẫn file CV PDF
    [CreatedAt]         DATETIME DEFAULT (GETDATE())         -- Ngày nộp hồ sơ
);
