
CREATE DATABASE QuanLyCuaHangBanXeMay;
GO

USE QuanLyCuaHangBanXeMay;
GO

-- 1. Tạo bảng Thể loại
CREATE TABLE tblTheloai(
    Maloai NVARCHAR(10) PRIMARY KEY,
    Tenloai NVARCHAR(50) NOT NULL
);

--2. Tạo bảng Động cơ
CREATE TABLE tblDongco(
    Madongco NVARCHAR(10) PRIMARY KEY,
    Tendongco NVARCHAR(50) NOT NULL
);
 
--3. Tạp bảng Màu sắc
CREATE TABLE tblMausac(
    Mamau NVARCHAR(10) PRIMARY KEY,
    Tenmau NVARCHAR(50) NOT NULL
);

--4. Tạo bảng Tình trạng
CREATE TABLE tblTinhtrang(
    Matinhtrang NVARCHAR(10) PRIMARY KEY,
    Tentinhtrang NVARCHAR(50) NOT NULL
);

--5. Tạo bảng Nước sản xuất
CREATE TABLE tblNuocsanxuat(
    Manuocsx NVARCHAR(10) PRIMARY KEY,
    Tennuocsx NVARCHAR(50) NOT NULL
);

--6. Tạo bảng Hãng sản xuất
CREATE TABLE tblHangsanxuat(
    Mahangsx NVARCHAR(10) PRIMARY KEY,
    Tenhangsx NVARCHAR(50) NOT NULL
);

--7. Tạo bảng Phanh xe
CREATE TABLE tblPhanhxe(
    Maphanh NVARCHAR(10) PRIMARY KEY,
    Tenphanh NVARCHAR(50) NOT NULL
);

--8. Tạo bảng Nhà cung cấp
CREATE TABLE tblNhacungcap(
    MaNCC NVARCHAR(10) PRIMARY KEY,
    TenNCC NVARCHAR(100) NOT NULL,
	Diachi NVARCHAR(100) NOT NULL,
	Dienthoai NVARCHAR(15) NOT NULL
);

--9. Tạo bảng Công việc
CREATE TABLE tblCongviec(
    MaCV NVARCHAR(10) PRIMARY KEY,
    TenCV NVARCHAR(50) NOT NULL,
	Luongthang FLOAT NOT NULL --Triệu VNĐ
);

--10. Tạo bảng Khách hàng
CREATE TABLE tblKhachhang(
    Makhach NVARCHAR(10) PRIMARY KEY,
    Tenkhach NVARCHAR(50) NOT NULL,
    Diachi NVARCHAR(100) NOT NULL,
    Dienthoai NVARCHAR(15) NOT NULL
);

--11. Tạo bảng Nhân viên
CREATE TABLE tblNhanvien(
    MaNV NVARCHAR(10) PRIMARY KEY,
    TenNV NVARCHAR(50) NOT NULL,
    Gioitinh NVARCHAR(10) NOT NULL,
	Ngaysinh DATE NOT NULL,
    Dienthoai NVARCHAR(15) NOT NULL,
	Diachi NVARCHAR(100) NOT NULL,
	MaCV NVARCHAR(10),
	Matkhau NVARCHAR(50) NOT NULL,
	FOREIGN KEY (MaCV) REFERENCES tblCongviec(MaCV)
);

-- 12. Tạo bảng Danh mục hàng
CREATE TABLE tblDMhang(
    Mahang NVARCHAR(10) PRIMARY KEY,
    Tenhang NVARCHAR(50) NOT NULL,
    Maloai NVARCHAR(10),
	Mahangsx NVARCHAR(10),
	Mamau NVARCHAR(10), 
	Namsanxuat INT,
	Maphanh NVARCHAR(10),
	Madongco NVARCHAR(10),
	Dungtichbinhxang FLOAT NOT NULL, 
	Manuocsx NVARCHAR(10),
	Matinhtrang NVARCHAR(10),
    Anh NVARCHAR(500) NOT NULL,
	Soluong INT,
    Dongianhap FLOAT NOT NULL,
    Dongiaban FLOAT NOT NULL,
    Thoigianbaohanh INT NOT NULL, -- số tháng
    FOREIGN KEY (Maloai) REFERENCES tblTheloai(Maloai),
	FOREIGN KEY (Mahangsx) REFERENCES tblHangsanxuat(Mahangsx),
	FOREIGN KEY (Mamau) REFERENCES tblMausac(Mamau),
	FOREIGN KEY (Maphanh) REFERENCES tblPhanhxe(Maphanh),
	FOREIGN KEY (Madongco) REFERENCES tblDongco(Madongco),
	FOREIGN KEY (Manuocsx) REFERENCES tblNuocsanxuat(Manuocsx),
	FOREIGN KEY (Matinhtrang) REFERENCES tblTinhtrang(Matinhtrang)
);


-- 13. Tạo bảng Đơn đặt hàng
CREATE TABLE tblDondathang(
    SoDDH NVARCHAR(10) PRIMARY KEY,
    MaNV NVARCHAR(10),
    MaKhach NVARCHAR(10),
    Ngaymua DATE NOT NULL,
    Datcoc FLOAT,
	Thue FLOAT,
	Tongtien FLOAT NOT NULL,
    FOREIGN KEY (MaNV) REFERENCES tblNhanvien(MaNV),
    FOREIGN KEY (Makhach) REFERENCES tblKhachhang(Makhach)
);

-- 14. Tạo bảng Chi tiết DDH
CREATE TABLE tblChitietDDH(
    SoDDH NVARCHAR(10),
    Mahang NVARCHAR(10),
    Soluong INT NOT NULL,
	Dongia INT,
    Giamgia FLOAT,
    Thanhtien FLOAT,
	PRIMARY KEY (SoDDH, Mahang),
	FOREIGN KEY (SoDDH) REFERENCES tblDondathang(SoDDH),
    FOREIGN KEY (Mahang) REFERENCES tblDMhang(Mahang)
);

--15. Tạo bảng Hóa đơn nhập
CREATE TABLE tblHoadonnhap(
    SoHDN NVARCHAR(10) PRIMARY KEY,
    MaNV NVARCHAR(10),
    Ngaynhap DATE NOT NULL,
    MaNCC NVARCHAR(10),
	Tongtien FLOAT,
    FOREIGN KEY (MaNV) REFERENCES tblNhanvien(MaNV),
    FOREIGN KEY (MaNCC) REFERENCES tblNhacungcap(MaNCC)
);

--16. Tạo bảng Chi tiết HDN
CREATE TABLE tblChitietHDN(
    SoHDN NVARCHAR(10),
    Mahang NVARCHAR(10),
    Soluong INT NOT NULL,
    Dongia FLOAT,
    Giamgia FLOAT,
    Thanhtien FLOAT,
    PRIMARY KEY (SoHDN, Mahang),
	FOREIGN KEY (SoHDN) REFERENCES tblHoadonnhap(SoHDN),
    FOREIGN KEY (Mahang) REFERENCES tblDMhang(Mahang)
);

---- Thêm các ràng buộc
ALTER TABLE tblDMhang
ADD 
    CONSTRAINT chk_Soluong CHECK (Soluong >= 0),
    CONSTRAINT chk_Dongianhap CHECK (Dongianhap >= 0),
    CONSTRAINT chk_Dongiaban CHECK (Dongiaban > 0),
    CONSTRAINT chk_Dungtichbinhxang CHECK (Dungtichbinhxang > 0),
    CONSTRAINT chk_Thoigianbaohanh CHECK (Thoigianbaohanh >= 0);


ALTER TABLE tblChitietDDH
ADD 
    CONSTRAINT chk_CTDDH_Soluong CHECK (Soluong >= 0),
    CONSTRAINT chk_CTDDH_Giamgia CHECK (Giamgia >= 0);
ALTER TABLE tblChitietHDN
ADD 
    CONSTRAINT chk_CTHDN_Soluong CHECK (Soluong >= 0),
    CONSTRAINT chk_CTHDN_Dongia CHECK (Dongia >= 0),
    CONSTRAINT chk_CTHDN_Giamgia CHECK (Giamgia >= 0);

ALTER TABLE tblDondathang
ADD 
    CONSTRAINT chk_DDH_Datcoc CHECK (Datcoc >= 0),
    CONSTRAINT chk_DDH_Thue CHECK (Thue >= 0),
    CONSTRAINT chk_DDH_Tongtien CHECK (Tongtien >= 0);

ALTER TABLE tblHoadonnhap
ADD 
    CONSTRAINT chk_HDN_Tongtien CHECK (Tongtien >= 0);


GO
CREATE TRIGGER trg_UpdateDongianhap_AfterInsert_HDN
ON tblChitietHDN
AFTER INSERT
AS
BEGIN
    UPDATE tblDMhang
    SET Dongianhap = i.Dongia
    FROM tblDMhang h
    JOIN inserted i ON h.Mahang = i.Mahang
END

--cập nhật đơn giá bán = 110% đơn giá nhập
CREATE TRIGGER trg_UpdateDongiaban_AfterUpdate_Dongianhap
ON tblDMhang
AFTER UPDATE
AS
BEGIN
    IF UPDATE(Dongianhap)
    BEGIN
        UPDATE tblDMhang
        SET Dongiaban = Dongianhap * 1.1
        WHERE Mahang IN (SELECT Mahang FROM inserted)
    END
END



-- cập nhật đơn giá bán 

CREATE TRIGGER trg_UpdateDongia_AfterInsert_DDH
ON tblChitietDDH
AFTER INSERT
AS
BEGIN
    UPDATE ctd
    SET ctd.Dongia = dm.Dongiaban
    FROM tblChitietDDH ctd
    JOIN inserted i ON ctd.SoDDH = i.SoDDH AND ctd.Mahang = i.Mahang
    JOIN tblDMhang dm ON ctd.Mahang = dm.Mahang
END



----Công thức tính thành tiền
GO
CREATE TRIGGER trg_TinhGiaVaThanhTien
ON tblChitietDDH
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
        -- Tính lại thành tiền
    UPDATE c
    SET
        ThanhTien = (c.SoLuong * c.Dongia) - c.GiamGia

    FROM tblChitietDDH c
    JOIN inserted i ON i.SoDDH = c.SoDDH AND i.Mahang = c.Mahang
END


GO
CREATE TRIGGER trg_CapNhatTongTien
ON tblChitietDDH
AFTER INSERT, UPDATE
AS
BEGIN
    UPDATE d
    SET d.TongTien = ISNULL(c.Tong, 0) + d.Thue - d.Datcoc
    FROM tblDondathang d 
    LEFT JOIN (
        SELECT SoDDH, SUM(c.Thanhtien) AS Tong
        FROM tblChitietDDH c
		GROUP BY SoDDH
    ) c ON d.SoDDH = c.SoDDH
END

--Cập nhật trigger đơn giá nhập
GO
CREATE TRIGGER trg_TinhGiaVaThanhTien_2
ON tblChitietHDN
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
        -- Tính lại thành tiền
    UPDATE c
    SET
        ThanhTien = (c.SoLuong * c.Dongia) - c.GiamGia

    FROM tblChitietHDN c
    JOIN inserted i ON i.SoHDN = c.SoHDN AND i.Mahang = c.Mahang
END

--
GO
CREATE TRIGGER trg_CapNhatTongTien2
ON tblChitietHDN 
AFTER INSERT, UPDATE
AS
BEGIN
    UPDATE h
    SET h.TongTien = ISNULL(c.Tong, 0) 
    FROM tblHoadonnhap h
    LEFT JOIN (
        SELECT SoHDN, SUM(c.Thanhtien) AS Tong
        FROM tblChitietHDN c
		GROUP BY SoHDN
    ) c ON c.SoHDN = h.SoHDN
END



--Chèn dữ liệu
--1.
INSERT INTO tblTheloai (Maloai, Tenloai) 
VALUES 
('TL01', N'Xe số'),
('TL02', N'Xe tay ga'),
('TL03', N'Xe côn tay'),
('TL04', N'Xe mô tô');

--2.
INSERT INTO tblDongco VALUES 
('DC01', N'110cc'), --TL01 - Xe số
('DC02', N'125cc'), --TL02 - Xe tay ga
('DC03', N'150cc'), --TL03 - Xe côn tay
('DC04', N'175cc'), --TL04 - Xe mô tô
('DC05', N'250cc'), --TL04 - Xe mô tô
('DC06', N'300cc'); --TL04 - Xe mô tô

--3.
INSERT INTO tblMausac VALUES 
('MS01', N'Đen'),
('MS02', N'Trắng'),
('MS03', N'Đỏ'),
('MS04', N'Xanh dương'),
('MS05', N'Vàng'),
('MS06', N'Xám'),
('MS07', N'Xanh lá cây'),
('MS08', N'Nâu'),
('MS09', N'Hồng'),
('MS10', N'Nâu');

--4. 
INSERT INTO tblTinhtrang VALUES 
('TT01', N'Mới 100%'),
('TT02', N'Đã qua sử dụng');


--5.
INSERT INTO tblNuocsanxuat VALUES 
('NSX01', N'Việt Nam'),
('NSX02', N'Nhật Bản'),
('NSX03', N'Thái Lan'),
('NSX04', N'Hàn Quốc'),
('NSX05', N'Trung Quốc'),
('NSX06', N'Anh'),
('NSX07', N'Mỹ'),
('NSX08', N'Ấn Độ'),
('NSX09', N'Indonesia'),
('NSX10', N'Malaysia');

--6.
INSERT INTO tblHangsanxuat VALUES
('HSX01', N'Yamaha'),
('HSX02', N'Honda'),
('HSX03', N'Suzuki'),
('HSX04', N'Kawasaki'),
('HSX05', N'Harley Davidson'),
('HSX06', N'Triumph'),
('HSX07', N'BMW'),
('HSX08', N'KTM'),
('HSX09', N'Vespa'),
('HSX10', N'Piaggio');

--7.
INSERT INTO tblPhanhxe VALUES 
('PX01', N'Phanh đĩa'),
('PX02', N'Phanh cơ');

--8. 
INSERT INTO tblNhacungcap VALUES
('NCC01', N'Công ty TNHH Xe máy Hoàng Anh', N'123 Trần Quang Diệu, Hà Nội', '0912345678'),
('NCC02', N'Công ty TNHH TMDV Nhật Minh', N'456 Lý Thường Kiệt, TP.HCM', '0987654321'),
('NCC03', N'Công ty TNHH Phát Tiến', N'789 Nguyễn Trãi, Đà Nẵng', '0908765432'),
('NCC04', N'Công ty Cổ phần An Hưng', N'101 Nguyễn Văn Cừ, Hải Phòng', '0911122334'),
('NCC05', N'Công ty TNHH Thế Giới Xe Máy', N'202 Lê Lợi, Cần Thơ', '0922334455'),
('NCC06', N'Công ty Cổ phần Xe Máy Sài Gòn', N'303 Nguyễn Thị Minh Khai, TP.HCM', '0933445566'),
('NCC07', N'Công ty TNHH Xe Máy Bình Dương', N'404 Đại lộ Bình Dương, Bình Dương', '0944556677'),
('NCC08', N'Công ty Cổ phần Xe Máy Hà Nội', N'505 Trần Hưng Đạo, Hà Nội', '0955667788'),
('NCC09', N'Công ty TNHH Xe Máy Thịnh Vượng', N'606 Hoàng Quốc Việt, Hà Nội', '0966778899'),
('NCC10', N'Công ty TNHH Xe Máy Xuân Vũ', N'707 Đoàn Kết, TP.HCM', '0977889900'),
('NCC11', N'Công ty TNHH Xe Máy Đức Thắng', N'808 Nguyễn Trãi, Hà Nội', '0988990011'),
('NCC12', N'Công ty Cổ phần Xe Máy Nam Anh', N'909 Lê Đại Hành, TP.HCM', '0999001122'),
('NCC13', N'Công ty TNHH Xe Máy Tiến Đạt', N'1010 Phan Đăng Lưu, Đà Nẵng', '0900112233');


--9. 
INSERT INTO tblCongviec VALUES 
('CV01', 'Quản lý cửa hàng', 20000000),
('CV02', 'Nhân viên bán hàng', 8000000),
('CV03', 'Kỹ thuật viên sửa chữa', 12000000),
('CV04', 'Nhân viên kho', 7000000),
('CV05', 'Nhân viên kế toán', 10000000),
('CV06', 'Nhân viên marketing', 9000000),
('CV07', 'Nhân viên hỗ trợ khách hàng', 8500000);

--10.
INSERT INTO tblKhachhang (Makhach, Tenkhach, Diachi, Dienthoai) VALUES
(N'KH001', N'Nguyễn Văn An', N'123 Đường Lê Lợi, Quận 1, TP. Hồ Chí Minh', N'0909123456'),
(N'KH002', N'Trần Thị Bích', N'45 Nguyễn Trãi, Quận 5, TP. Hồ Chí Minh', N'0918234567'),
(N'KH003', N'Lê Hoàng Long', N'78 Trần Hưng Đạo, Hà Nội', N'0937234123'),
(N'KH004', N'Phạm Minh Tuấn', N'10A Nguyễn Văn Cừ, TP. Đà Nẵng', N'0945123768'),
(N'KH005', N'Hoàng Thị Hạnh', N'5B Pasteur, Quận 3, TP. Hồ Chí Minh', N'0987345123'),
(N'KH006', N'Đặng Thị Mai', N'12 Đường Lý Thường Kiệt, TP. Huế', N'0903456789'),
(N'KH007', N'Ngô Văn Nam', N'99 Nguyễn Huệ, TP. Quy Nhơn', N'0912345678'),
(N'KH008', N'Thái Minh Quang', N'11 Trần Phú, TP. Nha Trang', N'0978123456'),
(N'KH009', N'Lê Thị Thu Hằng', N'3 Hùng Vương, TP. Cần Thơ', N'0934567890'),
(N'KH010', N'Phan Văn Dũng', N'34A Hai Bà Trưng, TP. Biên Hòa', N'0923456781'),
(N'KH011', N'Nguyễn Thị Thanh', N'27 Phạm Văn Đồng, TP. Vũng Tàu', N'0968123481'),
(N'KH012', N'Trương Văn Khánh', N'64 Nguyễn Văn Linh, TP. Đà Nẵng', N'0909988776'),
(N'KH013', N'Huỳnh Ngọc Hân', N'89 Lê Quý Đôn, TP. Pleiku', N'0911678932'),
(N'KH014', N'Đỗ Thị Tuyết', N'45 Đường Trường Chinh, TP. Hà Tĩnh', N'0987654321'),
(N'KH015', N'Võ Minh Hiếu', N'77 Phan Chu Trinh, TP. Rạch Giá', N'0902789456'),
(N'KH016', N'Nguyễn Hồng Sơn', N'50 Cách Mạng Tháng 8, TP. Hồ Chí Minh', N'0945567890'),
(N'KH017', N'Bùi Thị Lan', N'36 Trần Cao Vân, TP. Hội An', N'0912123456'),
(N'KH018', N'Trần Văn Bình', N'28 Nguyễn Đình Chiểu, TP. Phan Thiết', N'0939784562'),
(N'KH019', N'Phạm Thị Hòa', N'17B Đường Quang Trung, TP. Bắc Ninh', N'0902223334'),
(N'KH020', N'Ngô Văn Hậu', N'15 Hùng Vương, TP. Thanh Hóa', N'0976543210'),
(N'KH021', N'Trịnh Thị Hương', N'90 Lý Nam Đế, TP. Vinh', N'0913456789'),
(N'KH022', N'Đặng Văn Hùng', N'22 Nguyễn Trãi, TP. Hà Nội', N'0909234567'),
(N'KH023', N'Trần Thị Kim Chi', N'67 Pasteur, TP. Hồ Chí Minh', N'0987112233'),
(N'KH024', N'Lê Hoài Nam', N'101 Đường Tô Hiến Thành, TP. Đà Nẵng', N'0933123344'),
(N'KH025', N'Phạm Quang Vinh', N'56 Nguyễn Hữu Cảnh, TP. Thủ Đức', N'0923678912'),
(N'KH026', N'Nguyễn Thị Mỹ Linh', N'44 Phan Bội Châu, TP. Huế', N'0904123456'),
(N'KH027', N'Vũ Thanh Tú', N'32 Trường Sơn, TP. Hạ Long', N'0914123987'),
(N'KH028', N'Đỗ Trọng Nghĩa', N'28 Nguyễn Công Trứ, TP. Hà Giang', N'0962347890'),
(N'KH029', N'Nguyễn Hải Anh', N'17 Lê Lai, TP. Nam Định', N'0971892376'),
(N'KH030', N'Hồ Thị Diễm', N'81 Hoàng Diệu, TP. Bạc Liêu', N'0983456765'),
(N'KH031', N'Trần Minh Trí', N'53 Hùng Vương, TP. Buôn Ma Thuột', N'0905456789'),
(N'KH032', N'Nguyễn Thanh Tâm', N'27 Tôn Đức Thắng, TP. Long Xuyên', N'0916325874'),
(N'KH033', N'Phạm Ngọc Lâm', N'69 Hai Bà Trưng, TP. Thái Bình', N'0932345671'),
(N'KH034', N'Lý Thị Kim Oanh', N'35 Ngô Gia Tự, TP. Lạng Sơn', N'0908876123'),
(N'KH035', N'Huỳnh Văn Hòa', N'41 Nguyễn Văn Cừ, TP. Vũng Tàu', N'0945567800'),
(N'KH036', N'Nguyễn Văn Hưng', N'22 Lê Hồng Phong, TP. Ninh Bình', N'0912457890'),
(N'KH037', N'Trần Thị Mai Ly', N'14 Trần Hưng Đạo, TP. Phan Rang', N'0977435267'),
(N'KH038', N'Lê Quốc Khánh', N'99 Trần Phú, TP. Sơn La', N'0921245789'),
(N'KH039', N'Phan Thị Mỹ Dung', N'55 Nguyễn Huệ, TP. Quảng Ngãi', N'0935891245'),
(N'KH040', N'Bùi Trung Tín', N'88 Nguyễn Văn Linh, TP. Quảng Trị', N'0987123458'),
(N'KH041', N'Ngô Văn Tài', N'73 Nguyễn Thái Học, TP. Bắc Giang', N'0904761234'),
(N'KH042', N'Trịnh Minh Tùng', N'04 Trần Khánh Dư, TP. Thái Nguyên', N'0917845269'),
(N'KH043', N'Tạ Thị Tuyết Mai', N'62 Nguyễn Du, TP. Hải Phòng', N'0964812345'),
(N'KH044', N'Hồ Ngọc Duy', N'31B Phan Văn Trị, TP. Cần Thơ', N'0945879621'),
(N'KH045', N'Nguyễn Hữu Nghĩa', N'95 Trường Chinh, TP. Sóc Trăng', N'0903879123'),
(N'KH046', N'Lâm Văn Long', N'102 Đoàn Thị Điểm, TP. Trà Vinh', N'0928912367'),
(N'KH047', N'Võ Minh Khoa', N'88 Nguyễn An Ninh, TP. Tây Ninh', N'0987126458'),
(N'KH048', N'Trương Thị Hồng', N'21C Nguyễn Văn Trỗi, TP. Mỹ Tho', N'0934561200'),
(N'KH049', N'Hoàng Đức Lộc', N'39 Nguyễn Chí Thanh, TP. Hà Nội', N'0912774455'),
(N'KH050', N'Nguyễn Thị Hà', N'76 Đặng Thai Mai, TP. Hưng Yên', N'0978654123'),
(N'KH051', N'Phạm Văn Hảo', N'59 Ngô Quyền, TP. Lào Cai', N'0907658990'),
(N'KH052', N'Lê Thị Minh Nguyệt', N'68 Nguyễn Khuyến, TP. Thanh Hóa', N'0918654129'),
(N'KH053', N'Nguyễn Hồng Quang', N'11 Phạm Ngũ Lão, TP. Tuyên Quang', N'0984123874'),
(N'KH054', N'Trần Quang Đăng', N'22 Trần Văn Ơn, TP. Bắc Kạn', N'0967412300'),
(N'KH055', N'Thái Thị Thúy', N'35 Nguyễn Sinh Cung, TP. Hà Tĩnh', N'0938674532'),
(N'KH056', N'Đoàn Minh Hậu', N'40 Trần Phước Thành, TP. Bến Tre', N'0948888888'),
(N'KH057', N'Nguyễn Văn Lưu', N'57 Phan Văn Hớn, TP. Quảng Nam', N'0972345120'),
(N'KH058', N'Võ Thị Kim Chi', N'20 Nguyễn Duy Trinh, TP. Hà Nam', N'0987651230'),
(N'KH059', N'Phan Tấn Thành', N'31 Tạ Quang Bửu, TP. Đắk Nông', N'0902123451'),
(N'KH060', N'Ngô Trọng Nhân', N'44 Lê Trọng Tấn, TP. Gia Lai', N'0918234567');

--11. 
INSERT INTO tblNhanvien (MaNV, TenNV, Gioitinh, Ngaysinh, Dienthoai, Diachi, MaCV, Matkhau) VALUES
('NV01', N'Nguyễn Văn A', N'Nam', '1990-05-15', '0911222333', N'12 Lý Thường Kiệt, Hà Nội', 'CV01', 'asd1234'), -- CV01 - 1 nhân viên

('NV02', N'Lê Thị B', N'Nữ', '1993-08-22', '0922333444', N'34 Trần Hưng Đạo, TP.HCM', 'CV02', 'qwe5678'), -- CV02 - 2 nhân viên
('NV03', N'Ngô Văn C', N'Nam', '1991-03-10', '0909090909', N'56 Nguyễn Huệ, Đà Nẵng', 'CV02', 'zxc4321'),

('NV04', N'Trần Văn D', N'Nam', '1988-12-05', '0933444555', N'89 Nguyễn Trãi, Huế', 'CV03', 'plk8293'), -- CV03 - 2 nhân viên
('NV05', N'Phạm Thị E', N'Nữ', '1996-07-18', '0944555666', N'123 Trần Phú, Hải Phòng', 'CV03', 'bnm9201'),

('NV06', N'Hoàng Văn F', N'Nam', '1992-03-28', '0955666777', N'90 Cách Mạng Tháng Tám, Cần Thơ', 'CV04', 'cvb1123'),
('NV07', N'Nguyễn Thị G', N'Nữ', '1997-09-30', '0966777888', N'12 Nguyễn Văn Cừ, Hà Nội', 'CV04', 'mkl8734'),

('NV08', N'Đặng Văn H', N'Nam', '1990-01-10', '0977888999', N'123 Lê Văn Sỹ, TP.HCM', 'CV05', 'vbn4382'),
('NV09', N'Vũ Thị I', N'Nữ', '1994-06-11', '0988999000', N'78 Hai Bà Trưng, Hà Nội', 'CV05', 'xzs9021'),

('NV10', N'Phan Văn K', N'Nam', '1991-11-11', '0911999888', N'55 Điện Biên Phủ, TP.HCM', 'CV06', 'dsa7681'),
('NV11', N'Tạ Thị L', N'Nữ', '1995-05-05', '0922112233', N'101 Hoàng Hoa Thám, Huế', 'CV06', 'lkj3560'),

('NV12', N'Huỳnh Văn M', N'Nam', '1989-04-25', '0933555777', N'22 Nguyễn Đình Chiểu, Cần Thơ', 'CV07', 'qaz6390'),
('NV13', N'Lý Thị N', N'Nữ', '1998-10-19', '0944666888', N'88 Trần Quốc Toản, TP.HCM', 'CV07', 'wer1482');

--12.
INSERT INTO tblDMhang (Mahang, Tenhang, Maloai, Mahangsx, Mamau, Namsanxuat, Maphanh, Madongco, Dungtichbinhxang, Manuocsx, Matinhtrang, Anh, Soluong, Dongianhap, Dongiaban, Thoigianbaohanh) 
VALUES
('MH001', N'Yamaha FZ150', 'TL01', 'HSX01', 'MS01', 2022, 'PX01', 'DC01', 13.0, 'NSX01', 'TT01', 'images/yzf_r15.jpg', 10, 15000000, 20000000, 24),
('MH002', N'Honda SH150i', 'TL02', 'HSX02', 'MS02', 2022, 'PX01', 'DC02', 8.0, 'NSX02', 'TT01', 'images/honda_sh150i.jpg', 11, 20000000, 25000000, 24),
('MH003', N'Suzuki Gixxer SF', 'TL03', 'HSX03', 'MS03', 2023, 'PX02', 'DC03', 12.5, 'NSX03', 'TT01', 'images/suzuki_gixxer_sf.jpg', 12, 18000000, 22000000, 18),
('MH004', N'Kawasaki Ninja 300', 'TL04', 'HSX04', 'MS04', 2023, 'PX01', 'DC04', 17.0, 'NSX04', 'TT01', 'images/kawasaki_ninja300.jpg', 13, 25000000, 30000000, 36),
('MH005', N'Harley Davidson Sportster', 'TL04', 'HSX05', 'MS05', 2022, 'PX02', 'DC05', 22.0, 'NSX05', 'TT02', 'images/harley_sportster.jpg', 14, 35000000, 45000000, 48),
('MH006', N'Triumph Tiger 800', 'TL04', 'HSX06', 'MS06', 2023, 'PX01', 'DC06', 25.0, 'NSX06', 'TT01', 'images/triumph_tiger_800.jpg', 13, 40000000, 50000000, 36),
('MH007', N'BMW S1000RR', 'TL03', 'HSX07', 'MS07', 2022, 'PX01', 'DC06', 20.0, 'NSX07', 'TT01', 'images/bmw_s1000rr.jpg', 12, 45000000, 55000000, 48),
('MH008', N'KTM Duke 200', 'TL02', 'HSX08', 'MS08', 2022, 'PX02', 'DC02', 14.0, 'NSX08', 'TT01', 'images/ktm_duke_200.jpg', 11, 14000000, 18000000, 24),
('MH009', N'Vespa Primavera 125', 'TL02', 'HSX09', 'MS09', 2023, 'PX01', 'DC01', 10.0, 'NSX09', 'TT02', 'images/vespa_primavera.jpg', 10, 12000000, 15000000, 12),
('MH010', N'Piaggio Liberty 150', 'TL02', 'HSX10', 'MS10', 2022, 'PX01', 'DC02', 12.0, 'NSX10', 'TT01', 'images/piaggio_liberty.jpg', 9, 16000000, 20000000, 24);

--13. 
INSERT INTO tblDondathang VALUES (N'DDH001', N'NV04', N'KH012', N'2025-05-22', 5000000, 400000, 1);
INSERT INTO tblDondathang VALUES (N'DDH002', N'NV07', N'KH010', N'2025-05-29', 6000000, 300000, 1);
INSERT INTO tblDondathang VALUES (N'DDH003', N'NV08', N'KH057', N'2025-05-26', 2000000, 400000, 1);
INSERT INTO tblDondathang VALUES (N'DDH004', N'NV13', N'KH055', N'2025-05-24', 9000000, 200000, 1);
INSERT INTO tblDondathang VALUES (N'DDH005', N'NV03', N'KH050', N'2025-05-29', 5000000, 500000, 1);
INSERT INTO tblDondathang VALUES (N'DDH006', N'NV05', N'KH051', N'2025-05-16', 7000000, 100000, 1);
INSERT INTO tblDondathang VALUES (N'DDH007', N'NV07', N'KH059', N'2025-05-20', 3000000, 300000, 1);
INSERT INTO tblDondathang VALUES (N'DDH008', N'NV08', N'KH010', N'2025-05-23', 5000000, 300000, 1);
INSERT INTO tblDondathang VALUES (N'DDH009', N'NV11', N'KH024', N'2025-05-28', 6000000, 500000, 1);
INSERT INTO tblDondathang VALUES (N'DDH010', N'NV08', N'KH027', N'2025-05-03', 9000000, 300000, 1);
INSERT INTO tblDondathang VALUES (N'DDH011', N'NV13', N'KH014', N'2025-05-16', 8000000, 200000, 1);
INSERT INTO tblDondathang VALUES (N'DDH012', N'NV08', N'KH038', N'2025-05-12', 9000000, 300000, 1);
INSERT INTO tblDondathang VALUES (N'DDH013', N'NV11', N'KH015', N'2025-05-21', 4000000, 500000, 1);
INSERT INTO tblDondathang VALUES (N'DDH014', N'NV03', N'KH036', N'2025-05-07', 3000000, 100000, 1);
INSERT INTO tblDondathang VALUES (N'DDH015', N'NV04', N'KH007', N'2025-05-16', 5000000, 500000, 1);
INSERT INTO tblDondathang VALUES (N'DDH016', N'NV08', N'KH024', N'2025-05-28', 2000000, 400000, 1);
INSERT INTO tblDondathang VALUES (N'DDH017', N'NV05', N'KH043', N'2025-05-21', 2000000, 400000, 1);
INSERT INTO tblDondathang VALUES (N'DDH018', N'NV11', N'KH036', N'2025-05-11', 10000000, 200000, 1);
INSERT INTO tblDondathang VALUES (N'DDH019', N'NV12', N'KH005', N'2025-05-29', 2000000, 300000, 10);
INSERT INTO tblDondathang VALUES (N'DDH020', N'NV09', N'KH059', N'2025-05-02', 4000000, 100000, 01);
INSERT INTO tblDondathang VALUES (N'DDH021', N'NV02', N'KH038', N'2025-05-18', 7000000, 500000, 10);
INSERT INTO tblDondathang VALUES (N'DDH022', N'NV11', N'KH039', N'2025-05-18', 6000000, 100000, 01);
INSERT INTO tblDondathang VALUES (N'DDH023', N'NV08', N'KH039', N'2025-05-26', 4000000, 300000, 10);
INSERT INTO tblDondathang VALUES (N'DDH024', N'NV04', N'KH042', N'2025-05-09', 7000000, 400000, 01);
INSERT INTO tblDondathang VALUES (N'DDH025', N'NV01', N'KH025', N'2025-05-04', 6000000, 400000, 10);
INSERT INTO tblDondathang VALUES (N'DDH026', N'NV01', N'KH040', N'2025-05-27', 4000000, 500000, 01);
INSERT INTO tblDondathang VALUES (N'DDH027', N'NV06', N'KH023', N'2025-05-02', 8000000, 200000, 10);
INSERT INTO tblDondathang VALUES (N'DDH028', N'NV10', N'KH010', N'2025-05-26', 4000000, 500000, 01);
INSERT INTO tblDondathang VALUES (N'DDH029', N'NV10', N'KH009', N'2025-05-09', 2000000, 200000, 10);
INSERT INTO tblDondathang VALUES (N'DDH030', N'NV02', N'KH022', N'2025-05-04', 2000000, 100000, 01);
INSERT INTO tblDondathang VALUES (N'DDH031', N'NV06', N'KH058', N'2025-05-31', 2000000, 500000, 10);
INSERT INTO tblDondathang VALUES (N'DDH032', N'NV06', N'KH043', N'2025-05-18', 8000000, 200000, 01);
INSERT INTO tblDondathang VALUES (N'DDH033', N'NV06', N'KH053', N'2025-05-08', 6000000, 300000, 10);
INSERT INTO tblDondathang VALUES (N'DDH034', N'NV05', N'KH056', N'2025-05-22', 7000000, 400000, 01);
INSERT INTO tblDondathang VALUES (N'DDH035', N'NV13', N'KH005', N'2025-05-05', 4000000, 100000, 10);
INSERT INTO tblDondathang VALUES (N'DDH036', N'NV10', N'KH023', N'2025-05-16', 7000000, 400000, 10);
INSERT INTO tblDondathang VALUES (N'DDH037', N'NV01', N'KH007', N'2025-05-21', 3000000, 200000, 10);
INSERT INTO tblDondathang VALUES (N'DDH038', N'NV12', N'KH041', N'2025-05-25', 9000000, 400000, 10);
INSERT INTO tblDondathang VALUES (N'DDH039', N'NV11', N'KH018', N'2025-05-23', 8000000, 200000, 10);
INSERT INTO tblDondathang VALUES (N'DDH040', N'NV06', N'KH039', N'2025-05-27', 3000000, 100000, 10);
--14.
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH001', 'MH009', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH002', 'MH009', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH003', 'MH005', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH004', 'MH006', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH005', 'MH007', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH006', 'MH004', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH007', 'MH003', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH008', 'MH008', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH009', 'MH002', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH010', 'MH003', 1,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH012', 'MH009', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH011', 'MH006', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH013', 'MH004', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH013', 'MH007', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH014', 'MH004', 1,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH015', 'MH005', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH016', 'MH005', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH016', 'MH004', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH017', 'MH005', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH018', 'MH003', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH019', 'MH008', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH020', 'MH002', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH021', 'MH003', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH022', 'MH004', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH023', 'MH005', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH024', 'MH005', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH025', 'MH005', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH026', 'MH005', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH027', 'MH005', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH028', 'MH005', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH029', 'MH001', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH030', 'MH001', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH031', 'MH006', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH032', 'MH002', 1,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH033', 'MH001', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH034', 'MH001', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH035', 'MH005', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH036', 'MH002', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH037', 'MH005', 1,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH038', 'MH010', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH039', 'MH005', 1,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH001', 'MH007', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH002', 'MH006', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH003', 'MH010', 1,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH004', 'MH010', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH005', 'MH004', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH006', 'MH008', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH008', 'MH003', 1,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH009', 'MH006', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH024', 'MH001', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH010', 'MH002', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH038', 'MH003', 2,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH039', 'MH009', 3,1, 100000,1);
INSERT INTO tblChitietDDH (SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES (N'DDH040', 'MH006', 3,1, 100000,1);

--15.
INSERT INTO tblHoadonnhap (SoHDN, MaNV, Ngaynhap, MaNCC, Tongtien) VALUES
('HDN001', 'NV01', '2025-05-01', 'NCC01', 45000000),
('HDN002', 'NV02', '2025-05-02', 'NCC02', 60000000),
('HDN003', 'NV03', '2025-05-03', 'NCC03', 85000000),
('HDN004', 'NV04', '2025-05-04', 'NCC04', 50000000),
('HDN005', 'NV05', '2025-05-05', 'NCC05', 70000000),
('HDN006', 'NV06', '2025-05-06', 'NCC06', 64000000),
('HDN007', 'NV07', '2025-05-07', 'NCC07', 72000000),
('HDN008', 'NV08', '2025-05-08', 'NCC08', 51000000),
('HDN009', 'NV09', '2025-05-09', 'NCC09', 45000000),
('HDN010', 'NV10', '2025-05-10', 'NCC10', 60000000),
('HDN011', 'NV11', '2025-05-11', 'NCC11', 70000000),
('HDN012', 'NV12', '2025-05-12', 'NCC12', 55000000),
('HDN013', 'NV13', '2025-05-13', 'NCC13', 48000000),
('HDN014', 'NV01', '2025-05-14', 'NCC01', 67000000),
('HDN015', 'NV02', '2025-05-15', 'NCC02', 50000000),
('HDN016', 'NV03', '2025-05-16', 'NCC03', 73000000),
('HDN017', 'NV04', '2025-05-17', 'NCC04', 62000000),
('HDN018', 'NV05', '2025-05-18', 'NCC05', 56000000),
('HDN019', 'NV06', '2025-05-19', 'NCC06', 58000000);

--16.
INSERT INTO tblChitietHDN (SoHDN, Mahang, Soluong, Dongia, Giamgia, Thanhtien) VALUES
('HDN001', 'MH001', 2, 15000000, 0, 30000000),
('HDN001', 'MH002', 1, 20000000, 0, 20000000),

('HDN002', 'MH003', 2, 18000000, 0, 36000000),
('HDN002', 'MH004', 1, 24000000, 0, 24000000),

('HDN003', 'MH005', 2, 35000000, 5000000, 65000000),

('HDN004', 'MH006', 1, 40000000, 0, 40000000),
('HDN004', 'MH007', 1, 45000000, 5000000, 40000000),

('HDN005', 'MH008', 2, 14000000, 0, 28000000),
('HDN005', 'MH009', 1, 12000000, 0, 12000000),

('HDN006', 'MH010', 1, 16000000, 0, 16000000),
('HDN006', 'MH001', 2, 15000000, 1000000, 29000000),

('HDN007', 'MH002', 3, 20000000, 0, 60000000),

('HDN008', 'MH003', 2, 18000000, 6000000, 30000000),

('HDN009', 'MH004', 1, 25000000, 5000000, 20000000),

('HDN010', 'MH005', 1, 35000000, 0, 35000000),
('HDN010', 'MH006', 1, 40000000, 10000000, 30000000),

('HDN011', 'MH007', 2, 45000000, 5000000, 85000000),

('HDN012', 'MH008', 2, 14000000, 2000000, 26000000),

('HDN013', 'MH009', 1, 12000000, 0, 12000000),
('HDN013', 'MH010', 1, 16000000, 0, 16000000),

('HDN014', 'MH001', 1, 15000000, 0, 15000000),
('HDN014', 'MH002', 2, 20000000, 10000000, 30000000),

('HDN015', 'MH003', 1, 18000000, 0, 18000000),

('HDN016', 'MH004', 1, 25000000, 0, 25000000),

('HDN017', 'MH005', 1, 35000000, 0, 35000000),

('HDN018', 'MH006', 1, 40000000, 0, 40000000),

('HDN019', 'MH007', 1, 45000000, 0, 45000000);




------Phần làm báo cáo, chạy đoạn dưới
--- Báo cáo nhập hàng
CREATE PROCEDURE [dbo].[BCMatHangNhapTheoNgay]
    @Ngaynhap DateTime,
    @TuNgay DateTime ,
    @DenNgay DateTime 
AS
BEGIN
    SET NOCOUNT ON;
    IF @Ngaynhap IS NOT NULL
    BEGIN
        SELECT a.SoHDN
            ,a.Mahang
            ,b.Tenhang
            ,a.Soluong
            ,a.Dongia
            ,a.Giamgia
	,(a.Soluong*a.Dongia - a.Giamgia) as Thanhtien
            ,c.Ngaynhap
            ,d.TenNV
            ,e.TenNCC
        FROM tblChitietHDN as A
        JOIN tblDMhang as B ON a.Mahang = b.Mahang
        JOIN tblHoadonnhap as C ON a.SoHDN = c.SoHDN
        JOIN tblNhanvien as D ON c.MaNV = d.MaNV
        JOIN tblNhacungcap as E ON c.MaNCC = e.MaNCC
        WHERE convert(date,c.Ngaynhap) = @Ngaynhap
    END
    ELSE IF @TuNgay IS NOT NULL AND @DenNgay IS NOT NULL
    BEGIN
        SELECT a.SoHDN
            ,a.Mahang
            ,b.Tenhang
            ,a.Soluong
            ,a.Dongia
            ,a.Giamgia
	,(a.Soluong*a.Dongia  - a.Giamgia ) as Thanhtien
            ,c.Ngaynhap
            ,d.TenNV
            ,e.TenNCC
        FROM tblChitietHDN as A
        JOIN tblDMhang as B ON a.Mahang = b.Mahang
        JOIN tblHoadonnhap as C ON a.SoHDN = c.SoHDN
        JOIN tblNhanvien as D ON c.MaNV = d.MaNV
        JOIN tblNhacungcap as E ON c.MaNCC = e.MaNCC
        WHERE convert(date,c.Ngaynhap) BETWEEN @TuNgay AND @DenNgay
    END
END

CREATE PROCEDURE [dbo].[TongMHNhapTheoNgay]
    @Ngaynhap DateTime,
    @TuNgay DateTime ,
    @DenNgay DateTime
AS
BEGIN
    SET NOCOUNT ON;
    IF @Ngaynhap IS NOT NULL
    BEGIN
        SELECT a.Mahang
            ,b.Tenhang
            ,SUM(a.Soluong) AS TongSoLuong
        FROM tblChitietHDN AS a
        JOIN tblDMhang AS b ON a.Mahang = b.Mahang
        JOIN tblHoadonnhap AS c ON a.SoHDN = c.SoHDN
        WHERE c.Ngaynhap = @Ngaynhap
        GROUP BY a.Mahang, b.Tenhang;
    END
    ELSE IF @TuNgay IS NOT NULL AND @DenNgay IS NOT NULL
    BEGIN
        SELECT a.Mahang
            ,b.Tenhang
            ,SUM(a.Soluong) AS TongSoLuong
        FROM tblChitietHDN AS a
        JOIN tblDMhang AS b ON a.Mahang = b.Mahang
        JOIN tblHoadonnhap AS c ON a.SoHDN = c.SoHDN
        WHERE c.Ngaynhap BETWEEN @TuNgay AND @DenNgay
        GROUP BY a.Mahang, b.Tenhang;
    END
END


-- Bán hàng
CREATE PROCEDURE [dbo].[BCBanHang]
    @Ngayban DateTime,
    @TuNgay DateTime,
    @DenNgay DateTime
AS
BEGIN
    SET NOCOUNT ON;
    IF @Ngayban IS NOT NULL
    BEGIN
        SELECT 
		b.Ngaymua AS NgayBan,
		a.SoDDH,
		a.Mahang,
		c.Tenhang,
		c.Dongiaban,
		a.Soluong AS SoLuongBanDuoc,
		a.Giamgia,
		(a.Soluong * c.Dongiaban - a.Giamgia) AS Thanhtien,
		kh.Tenkhach,
		nv.TenNV
		FROM 
			tblChitietDDH AS a
		INNER JOIN 
			tblDondathang AS b ON a.SoDDH = b.SoDDH
		INNER JOIN 
			tblDMhang AS c ON a.Mahang = c.Mahang
		INNER JOIN 
			tblKhachHang AS kh ON b.Makhach = kh.Makhach
		INNER JOIN 
			tblNhanvien as nv on b.MaNV=nv.MaNV
		WHERE CONVERT(date, b.Ngaymua) = @Ngayban
	END
    ELSE IF @TuNgay IS NOT NULL AND @DenNgay IS NOT NULL
    BEGIN
        SELECT b.Ngaymua AS NgayBan,
		a.SoDDH,
		a.Mahang,
		c.Tenhang,
		c.Dongiaban,
		a.Soluong AS SoLuongBanDuoc,
		a.Giamgia,
		(a.Soluong * c.Dongiaban - a.Giamgia) AS Thanhtien,
		kh.Tenkhach,
		nv.TenNV
		FROM 
			tblChitietDDH AS a
		INNER JOIN 
			tblDondathang AS b ON a.SoDDH = b.SoDDH
		INNER JOIN 
			tblDMhang AS c ON a.Mahang = c.Mahang
		INNER JOIN 
			tblKhachHang AS kh ON b.Makhach = kh.Makhach
		INNER JOIN 
			tblNhanvien as nv on b.MaNV=nv.MaNV
        WHERE convert(date,b.Ngaymua) BETWEEN @TuNgay AND @DenNgay
    END
END

CREATE PROCEDURE [dbo].[TongMHBan]
    @Ngayban DateTime,
    @TuNgay DateTime ,
    @DenNgay DateTime
AS
BEGIN
    SET NOCOUNT ON;
    IF @Ngayban IS NOT NULL
    BEGIN
        SELECT a.Mahang
            ,b.Tenhang
            ,SUM(a.Soluong) AS TongSoLuong
        FROM tblChitietDDH AS a
        JOIN tblDMhang AS b ON a.Mahang = b.Mahang
        JOIN tblDondathang AS c ON a.SoDDH = c.SoDDH
        WHERE c.Ngaymua = @Ngayban
        GROUP BY a.Mahang, b.Tenhang;
    END
    ELSE IF @TuNgay IS NOT NULL AND @DenNgay IS NOT NULL
    BEGIN
        SELECT a.Mahang
            ,b.Tenhang
            ,SUM(a.Soluong) AS TongSoLuong
        FROM tblChitietDDH AS a
        JOIN tblDMhang AS b ON a.Mahang = b.Mahang
        JOIN tblDondathang AS c ON a.SoDDH = c.SoDDH
        WHERE c.Ngaymua  BETWEEN @TuNgay AND @DenNgay
        GROUP BY a.Mahang, b.Tenhang;
    END
END
 

-- Doanh thu
CREATE PROCEDURE [dbo].[BCDoanhThu]
    @Ngayban DateTime,
    @TuNgay DateTime,
    @DenNgay DateTime
AS
BEGIN
    SET NOCOUNT ON;
    IF @Ngayban IS NOT NULL
    BEGIN
        SELECT a.Mahang
	, c.Tenhang
            , c.Dongiaban
	, SUM(a.Soluong) AS SoLuongBanDuoc
            , SUM(a.Soluong * c.Dongiaban - a.Giamgia) AS DoanhThu
        FROM tblChitietDDH AS a
        INNER JOIN tblDondathang AS b ON a.SoDDH = b.SoDDH
        INNER JOIN tblDMhang AS c ON a.Mahang = c.Mahang
        WHERE convert(date,b.Ngaymua) = @Ngayban
        GROUP BY a.Mahang, c.Tenhang, c.Dongiaban;
    END
    ELSE IF @TuNgay IS NOT NULL AND @DenNgay IS NOT NULL
    BEGIN
        SELECT a.Mahang
            , c.Tenhang
            , c.Dongiaban
            , SUM(a.Soluong * c.Dongiaban - a.Giamgia*0.01) AS DoanhThu
            , SUM(a.Soluong) AS SoLuongBanDuoc
        FROM tblChitietDDH AS a
        INNER JOIN tblDondathang AS b ON a.SoDDH = b.SoDDH
        INNER JOIN tblDMhang AS c ON a.Mahang = c.Mahang
        WHERE convert(date,b.Ngaymua) BETWEEN @TuNgay AND @DenNgay
        GROUP BY a.Mahang,c.Tenhang, c.Dongiaban;
    END
END


---Chạy thử
EXEC BCMatHangNhapTheoNgay @Ngaynhap = '2025-05-01', @TuNgay = NULL, @DenNgay = NULL;
-- Test theo khoảng ngày:
EXEC BCMatHangNhapTheoNgay @Ngaynhap = NULL, @TuNgay = '2025-05-01', @DenNgay = '2025-05-29';

-- Test theo ngày cụ thể:
EXEC TongMHNhapTheoNgay @Ngaynhap = '2025-05-01', @TuNgay = NULL, @DenNgay = NULL;
-- Test theo khoảng ngày:
EXEC TongMHNhapTheoNgay @Ngaynhap = NULL, @TuNgay = '2025-05-01', @DenNgay = '2025-05-29';

-- Test theo ngày cụ thể:
EXEC BCBanHang @Ngayban = '2025-05-16', @TuNgay = NULL, @DenNgay = NULL;
-- Test theo khoảng ngày:
EXEC BCBanHang @Ngayban = NULL, @TuNgay = '2025-05-01', @DenNgay = '2025-05-29';

-- Test theo ngày cụ thể:
EXEC TongMHBan @Ngayban = '2025-05-16', @TuNgay = NULL, @DenNgay = NULL;
-- Test theo khoảng ngày:
EXEC TongMHBan @Ngayban = NULL, @TuNgay = '2025-05-01', @DenNgay = '2025-05-29';

-- Test theo ngày cụ thể:
EXEC BCDoanhThu @Ngayban = '2025-05-18', @TuNgay = NULL, @DenNgay = NULL;
-- Test theo khoảng ngày:
EXEC BCDoanhThu @Ngayban = NULL, @TuNgay = '2025-05-07', @DenNgay = '2025-05-29';
