CREATE DATABASE VR_escape_rooms;
GO

USE VR_escape_rooms;
GO

CREATE TABLE Korisnik (
    ID INT PRIMARY KEY IDENTITY(1,1),
    ime NVARCHAR(50),
    email NVARCHAR(100) UNIQUE,
    lozinka NVARCHAR(255),
    tip_korisnika NVARCHAR(10) CHECK (tip_korisnika IN ('admin', 'igrac'))
);
GO

CREATE TABLE Igra (
    ID INT PRIMARY KEY IDENTITY(1,1),
    naziv NVARCHAR(100),
    težina NVARCHAR(10) CHECK (težina IN ('lako', 'srednje', 'teško')),
    trajanje INT,
    opis NVARCHAR(MAX)
);
GO

CREATE TABLE Rezervacija (
    ID INT PRIMARY KEY IDENTITY(1,1),
    korisnik_id INT,
    igra_id INT,
    datum_rezervacije DATETIME,
    status NVARCHAR(10) CHECK (status IN ('aktivna', 'otkazana')),
    FOREIGN KEY (korisnik_id) REFERENCES Korisnik(ID) ON DELETE CASCADE,
    FOREIGN KEY (igra_id) REFERENCES Igra(ID) ON DELETE CASCADE
);
GO

CREATE TABLE Ocjena (
    ID INT PRIMARY KEY IDENTITY(1,1),
    korisnik_id INT,
    igra_id INT,
    ocjena INT CHECK (ocjena BETWEEN 1 AND 5),
    komentar NVARCHAR(MAX),
    FOREIGN KEY (korisnik_id) REFERENCES Korisnik(ID) ON DELETE CASCADE,
    FOREIGN KEY (igra_id) REFERENCES Igra(ID) ON DELETE CASCADE
);
GO

CREATE TABLE PrijavaKorisnika (
    ID INT PRIMARY KEY IDENTITY(1,1),
    prijavio_id INT,
    prijavljeni_id INT,
    razlog NVARCHAR(MAX) NOT NULL,
    status NVARCHAR(15) CHECK (status IN ('na cekanju', 'odobreno', 'odbijeno')),
    datum_prijave DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (prijavio_id) REFERENCES Korisnik(ID) ON DELETE NO ACTION,
    FOREIGN KEY (prijavljeni_id) REFERENCES Korisnik(ID) ON DELETE NO ACTION
);
GO

INSERT INTO [Korisnik] (ime, email, lozinka, tip_korisnika)  
VALUES  
  ('Amar Dedić', 'amar.dedic@email.com', 'pass12345', 'admin'),  
  ('Lejla Hadžić', 'lejla.hadzic@hotmail.com', 'qwerty6789', 'igrac'),  
  ('Haris Mujkić', 'haris.mujkic@google.org', 'escapeMe42!', 'igrac'),  
  ('Adnan Begović', 'adnan.begovic@hotmail.ba', 'gameMaster777', 'igrac'),  
  ('Selma Zukić', 'selma.zukic@aol.com', 'p@sswordXYZ', 'admin');  

INSERT INTO [Igra] (naziv, težina, trajanje, opis)  
VALUES  
  ('Haunted Mansion', 'teško', 60, 'Igra u ukletoj kući sa duhovima'),  
  ('Space Explorer', 'srednje', 45, 'Istraživanje svemira i pronalazak izgubljene planete'),  
  ('Ancient Pyramid', 'lako', 30, 'Pokušaj pobjeći iz piramide prije nego što se uruši'),  
  ('Cyber Heist', 'srednje', 50, 'Hakuj sistem i ukradi podatke prije nego što te otkriju'),  
  ('Jungle Escape', 'teško', 70, 'Preživi u džungli i pronađi put do civilizacije');  

INSERT INTO [Rezervacija] (korisnik_id, igra_id, datum_rezervacije, status)  
VALUES  
  (2, 1, '2024-02-06 15:00:00', 'aktivna'),  
  (3, 2, '2024-02-07 17:30:00', 'otkazana'),  
  (4, 3, '2024-02-08 19:00:00', 'aktivna'),  
  (5, 4, '2024-02-09 14:45:00', 'aktivna'),  
  (3, 5, '2024-02-10 20:00:00', 'otkazana');  

INSERT INTO [Ocjena] (korisnik_id, igra_id, ocjena, komentar)  
VALUES  
  (2, 1, 5, 'Fenomenalno iskustvo! Osjećao/la sam se kao u pravoj ukletoj kući.'),  
  (3, 2, 4, 'Zabavno, ali malo previše lagano za moj ukus.'),  
  (4, 3, 3, 'Dobar koncept, ali mogli su dodati više zagonetki.'),  
  (5, 4, 5, 'Adrenalinska igra, preporučujem svima!'),  
  (3, 5, 2, 'Težak nivo, ali grafika i atmosfera su odlični.');  

INSERT INTO [PrijavaKorisnika] (prijavio_id, prijavljeni_id, razlog, status, datum_prijave)  
VALUES  
  (2, 3, 'Vulgarnost u chatu tokom igre', 'na cekanju', '2024-02-06'),  
  (3, 2, 'Nepoštovanje pravila igre', 'odobreno', '2024-02-05'),  
  (4, 5, 'Neprikladno ponašanje prema timu', 'odbijeno', '2024-02-04'),  
  (5, 3, 'Namjerno sabotiranje igre', 'na cekanju', '2024-02-03'),  
  (3, 4, 'Spamovanje poruka u globalnom chatu', 'odobreno', '2024-02-02');  

ALTER TABLE Korisnik ADD HighScore INT DEFAULT 0;
ALTER TABLE Korisnik ADD MaxScore INT DEFAULT 0;

UPDATE Korisnik
SET HighScore = CASE 
                    WHEN ime = 'Amar Dedić' THEN 150
                    WHEN ime = 'Lejla Hadžić' THEN 120
                    WHEN ime = 'Haris Mujkić' THEN 80
                    WHEN ime = 'Adnan Begović' THEN 200
                    WHEN ime = 'Selma Zukić' THEN 130
                    ELSE HighScore
                END,
    MaxScore = CASE 
                    WHEN ime = 'Amar Dedić' THEN 220
                    WHEN ime = 'Lejla Hadžić' THEN 180
                    WHEN ime = 'Haris Mujkić' THEN 150
                    WHEN ime = 'Adnan Begović' THEN 250
                    WHEN ime = 'Selma Zukić' THEN 200
                    ELSE MaxScore
                END;

ALTER TABLE Igra ADD Zanr NVARCHAR(50) DEFAULT 'nepoznato';

UPDATE Igra SET Zanr = 'horor' WHERE naziv = 'Haunted Mansion';
UPDATE Igra SET Zanr = 'sci-fi' WHERE naziv = 'Space Explorer';
UPDATE Igra SET Zanr = 'avantura' WHERE naziv = 'Ancient Pyramid';
UPDATE Igra SET Zanr = 'akcija' WHERE naziv = 'Cyber Heist';
UPDATE Igra SET Zanr = 'preživljavanje' WHERE naziv = 'Jungle Escape';

SELECT * FROM Igra;

SELECT * FROM Korisnik;
