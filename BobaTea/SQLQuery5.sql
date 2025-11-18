CREATE TABLE [dbo].[CustomerSupport] (
    [SupportId]     INT IDENTITY(1,1) PRIMARY KEY,   -- Khóa chính, tự tăng
    [Name]          NVARCHAR(100) NOT NULL,          -- Họ tên người liên hệ
    [Email]         NVARCHAR(100) NOT NULL,          -- Email người liên hệ
    [Message]       NVARCHAR(MAX) NOT NULL,          -- Nội dung chăm sóc / phản hồi
    [CreatedAt]     DATETIME DEFAULT (GETDATE())     -- Ngày gửi
);
