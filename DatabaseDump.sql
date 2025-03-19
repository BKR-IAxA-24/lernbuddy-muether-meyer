-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Mar 19, 2025 at 11:38 AM
-- Server version: 8.0.30
-- PHP Version: 8.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `nachhilfedb`
--
CREATE DATABASE IF NOT EXISTS `nachhilfedb` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_de_pb_0900_ai_ci;
USE `nachhilfedb`;

-- --------------------------------------------------------

--
-- Table structure for table `bildungsgang`
--

CREATE TABLE `bildungsgang` (
  `BildungsgangID` int NOT NULL,
  `Bezeichnung` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_de_pb_0900_ai_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_de_pb_0900_ai_ci;

--
-- Dumping data for table `bildungsgang`
--

INSERT INTO `bildungsgang` (`BildungsgangID`, `Bezeichnung`) VALUES
(1, 'Staatlich geprüfte Informationstechnische Assistentin/Staatlich geprüfter Informationstechnischer Assistent'),
(2, 'Berufliches Gymnasium – Bautechnik'),
(3, 'Ausbildungsvorbereitung Ernährungs- und Versorgungsmanagement'),
(4, 'Elektronikerin/Elektroniker für Betriebstechnik'),
(5, 'Fachoberschule Gesundheit und Soziales');

-- --------------------------------------------------------

--
-- Table structure for table `fach`
--

CREATE TABLE `fach` (
  `FachID` int NOT NULL,
  `Bezeichnung` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_de_pb_0900_ai_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_de_pb_0900_ai_ci;

--
-- Dumping data for table `fach`
--

INSERT INTO `fach` (`FachID`, `Bezeichnung`) VALUES
(1, 'Mathematik'),
(2, 'Deutsch'),
(3, 'Englisch'),
(4, 'Datenbanken'),
(5, 'Software'),
(6, 'Elektrotechnik');

-- --------------------------------------------------------

--
-- Table structure for table `klasse`
--

CREATE TABLE `klasse` (
  `KlassenID` int NOT NULL,
  `Bezeichnung` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_de_pb_0900_ai_ci NOT NULL,
  `BID` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_de_pb_0900_ai_ci;

--
-- Dumping data for table `klasse`
--

INSERT INTO `klasse` (`KlassenID`, `Bezeichnung`, `BID`) VALUES
(1, 'IA1A', 1),
(2, 'IA1B', 1),
(3, 'IA2A', 1),
(4, 'IA2B', 1),
(5, 'IA3A', 1),
(6, 'IA3B', 1),
(7, 'G1B', 2),
(8, 'G2B', 2),
(9, 'G3B', 2);

-- --------------------------------------------------------

--
-- Table structure for table `lehrerhatfach`
--

CREATE TABLE `lehrerhatfach` (
  `TutorID` int NOT NULL,
  `FachID` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_de_pb_0900_ai_ci;

--
-- Dumping data for table `lehrerhatfach`
--

INSERT INTO `lehrerhatfach` (`TutorID`, `FachID`) VALUES
(1, 4),
(1, 5);

-- --------------------------------------------------------

--
-- Table structure for table `login`
--

CREATE TABLE `login` (
  `LoginID` int NOT NULL,
  `E-Mail` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_de_pb_0900_ai_ci NOT NULL,
  `Passwort` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_de_pb_0900_ai_ci NOT NULL,
  `admin` tinyint(1) NOT NULL,
  `salt` text COLLATE utf8mb4_de_pb_0900_ai_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_de_pb_0900_ai_ci;

--
-- Dumping data for table `login`
--

INSERT INTO `login` (`LoginID`, `E-Mail`, `Passwort`, `admin`, `salt`) VALUES
(1, 'admin@berufskolleg-rheine.de', 'admin', 1, ''),
(2, '71642@bkrnet.de', 'Felix!1999', 0, ''),
(3, '64285@bkrnet.de', 'Pass!123', 0, '');

-- --------------------------------------------------------

--
-- Table structure for table `nachhilfegesuch`
--

CREATE TABLE `nachhilfegesuch` (
  `GesuchID` int NOT NULL,
  `SchuelerID` int NOT NULL,
  `FachID` int DEFAULT NULL,
  `Beschreibung` text CHARACTER SET utf8mb4 COLLATE utf8mb4_de_pb_0900_ai_ci,
  `ErstelltAm` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `Status` enum('offen','erledigt') CHARACTER SET utf8mb4 COLLATE utf8mb4_de_pb_0900_ai_ci NOT NULL DEFAULT 'offen'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_de_pb_0900_ai_ci;

--
-- Dumping data for table `nachhilfegesuch`
--

INSERT INTO `nachhilfegesuch` (`GesuchID`, `SchuelerID`, `FachID`, `Beschreibung`, `ErstelltAm`, `Status`) VALUES
(1, 1, 1, 'Brauche dringend Hilfe in Mathe vor der Klausur.', '2025-01-15 13:30:00', 'offen'),
(2, 4, 3, 'Verstehe die englische Grammatik nicht, bitte um Unterstützung.', '2025-02-10 09:15:00', 'offen'),
(3, 6, 6, 'Suche Nachhilfe in Elektrotechnik für meine Prüfung.', '2025-03-05 15:00:00', 'offen'),
(4, 17, 2, 'Ich komme mit der deutschen Rechtschreibung nicht klar.', '2025-01-28 08:45:00', 'offen'),
(5, 13, 4, 'Brauche Hilfe in Datenbanken für ein Projekt.', '2025-02-25 16:20:00', 'erledigt'),
(6, 13, 5, 'Softwareentwicklung ist kompliziert, wer kann helfen?', '2025-03-02 14:10:00', 'offen'),
(7, 12, 1, 'Mathe ist eine Katastrophe. Ich brauche dringend Hilfe!', '2025-01-22 10:35:00', 'offen'),
(8, 10, 3, 'Englisch Konversation üben für eine mündliche Prüfung.', '2025-02-18 12:00:00', 'erledigt');

-- --------------------------------------------------------

--
-- Table structure for table `schueler`
--

CREATE TABLE `schueler` (
  `SchuelerID` int NOT NULL,
  `Vorname` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_de_pb_0900_ai_ci NOT NULL,
  `Nachname` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_de_pb_0900_ai_ci NOT NULL,
  `Geschlecht` enum('m','w','d') CHARACTER SET utf8mb4 COLLATE utf8mb4_de_pb_0900_ai_ci NOT NULL,
  `Email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_de_pb_0900_ai_ci DEFAULT NULL,
  `KlassenID` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_de_pb_0900_ai_ci;

--
-- Dumping data for table `schueler`
--

INSERT INTO `schueler` (`SchuelerID`, `Vorname`, `Nachname`, `Geschlecht`, `Email`, `KlassenID`) VALUES
(1, 'Max', 'Müller', 'm', '38427@bkrnet.de', 3),
(2, 'Leonie', 'Schmidt', 'w', '52918@bkrnet.de', 7),
(3, 'Felix', 'Weber', 'm', '71642@bkrnet.de', 5),
(4, 'Elif', 'Kaya', 'w', '85273@bkrnet.de', 8),
(5, 'Tim', 'Becker', 'm', '49361@bkrnet.de', 6),
(6, 'Julia', 'Hoffmann', 'w', '15739@bkrnet.de', 1),
(7, 'Lukas', 'Schwarz', 'm', '64925@bkrnet.de', 4),
(8, 'Nina', 'Krüger', 'w', '23847@bkrnet.de', 9),
(9, 'Moritz', 'Maier', 'm', '91573@bkrnet.de', 6),
(10, 'Sophie', 'Wagner', 'w', '75632@bkrnet.de', 9),
(11, 'Jonas', 'Krause', 'm', '48726@bkrnet.de', 5),
(12, 'Laura', 'Fischer', 'w', '64285@bkrnet.de', 7),
(13, 'Leon', 'Richter', 'm', '83924@bkrnet.de', 2),
(14, 'Zeynep', 'Şahin', 'w', '97351@bkrnet.de', 8),
(15, 'Elias', 'Braun', 'm', '21597@bkrnet.de', 1),
(16, 'Hannah', 'Kühn', 'w', '46823@bkrnet.de', 7),
(17, 'Omar', 'Hassan', 'm', '59281@bkrnet.de', 1),
(18, 'Paul', 'Voigt', 'm', '71349@bkrnet.de', 8),
(19, 'Ali', 'Yılmaz', 'm', '32578@bkrnet.de', 3),
(20, 'Sara', 'Mahmoud', 'w', '85741@bkrnet.de', 9);

-- --------------------------------------------------------

--
-- Table structure for table `tutor`
--

CREATE TABLE `tutor` (
  `TutorID` int NOT NULL,
  `SchuelerID` int NOT NULL,
  `Genehmigt` float NOT NULL DEFAULT '0',
  `LoginID` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_de_pb_0900_ai_ci;

--
-- Dumping data for table `tutor`
--

INSERT INTO `tutor` (`TutorID`, `SchuelerID`, `Genehmigt`, `LoginID`) VALUES
(1, 3, 1, 2),
(2, 12, 1, 3);

-- --------------------------------------------------------

--
-- Table structure for table `wochentage`
--

CREATE TABLE `wochentage` (
  `WTID` int NOT NULL,
  `Tag` text COLLATE utf8mb4_de_pb_0900_ai_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_de_pb_0900_ai_ci;

--
-- Dumping data for table `wochentage`
--

INSERT INTO `wochentage` (`WTID`, `Tag`) VALUES
(1, 'Montag'),
(2, 'Dienstag'),
(3, 'Mittwoch'),
(4, 'Donnerstag'),
(5, 'Freitag'),
(6, 'Samstag'),
(7, 'Sonntag');

-- --------------------------------------------------------

--
-- Table structure for table `zeitspanne`
--

CREATE TABLE `zeitspanne` (
  `WTID` int NOT NULL,
  `SID` int NOT NULL,
  `Start` text COLLATE utf8mb4_de_pb_0900_ai_ci NOT NULL,
  `Ende` text COLLATE utf8mb4_de_pb_0900_ai_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_de_pb_0900_ai_ci;

--
-- Dumping data for table `zeitspanne`
--

INSERT INTO `zeitspanne` (`WTID`, `SID`, `Start`, `Ende`) VALUES
(3, 1, '1700', '1800');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `bildungsgang`
--
ALTER TABLE `bildungsgang`
  ADD PRIMARY KEY (`BildungsgangID`);

--
-- Indexes for table `fach`
--
ALTER TABLE `fach`
  ADD PRIMARY KEY (`FachID`);

--
-- Indexes for table `klasse`
--
ALTER TABLE `klasse`
  ADD PRIMARY KEY (`KlassenID`),
  ADD KEY `BID` (`BID`);

--
-- Indexes for table `lehrerhatfach`
--
ALTER TABLE `lehrerhatfach`
  ADD PRIMARY KEY (`TutorID`,`FachID`),
  ADD KEY `FachID` (`FachID`);

--
-- Indexes for table `login`
--
ALTER TABLE `login`
  ADD PRIMARY KEY (`LoginID`);

--
-- Indexes for table `nachhilfegesuch`
--
ALTER TABLE `nachhilfegesuch`
  ADD PRIMARY KEY (`GesuchID`),
  ADD KEY `FachID` (`FachID`),
  ADD KEY `SchuelerID` (`SchuelerID`);

--
-- Indexes for table `schueler`
--
ALTER TABLE `schueler`
  ADD PRIMARY KEY (`SchuelerID`),
  ADD UNIQUE KEY `Email` (`Email`),
  ADD KEY `KlassenID` (`KlassenID`);

--
-- Indexes for table `tutor`
--
ALTER TABLE `tutor`
  ADD PRIMARY KEY (`TutorID`,`SchuelerID`),
  ADD KEY `SchuelerID` (`SchuelerID`),
  ADD KEY `LoginID` (`LoginID`);

--
-- Indexes for table `wochentage`
--
ALTER TABLE `wochentage`
  ADD PRIMARY KEY (`WTID`);

--
-- Indexes for table `zeitspanne`
--
ALTER TABLE `zeitspanne`
  ADD PRIMARY KEY (`WTID`,`SID`),
  ADD KEY `SID` (`SID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `bildungsgang`
--
ALTER TABLE `bildungsgang`
  MODIFY `BildungsgangID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `fach`
--
ALTER TABLE `fach`
  MODIFY `FachID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `klasse`
--
ALTER TABLE `klasse`
  MODIFY `KlassenID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `login`
--
ALTER TABLE `login`
  MODIFY `LoginID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `nachhilfegesuch`
--
ALTER TABLE `nachhilfegesuch`
  MODIFY `GesuchID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `schueler`
--
ALTER TABLE `schueler`
  MODIFY `SchuelerID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `klasse`
--
ALTER TABLE `klasse`
  ADD CONSTRAINT `klasse_ibfk_1` FOREIGN KEY (`BID`) REFERENCES `bildungsgang` (`BildungsgangID`);

--
-- Constraints for table `lehrerhatfach`
--
ALTER TABLE `lehrerhatfach`
  ADD CONSTRAINT `lehrerhatfach_ibfk_1` FOREIGN KEY (`TutorID`) REFERENCES `tutor` (`TutorID`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `lehrerhatfach_ibfk_2` FOREIGN KEY (`FachID`) REFERENCES `fach` (`FachID`);

--
-- Constraints for table `nachhilfegesuch`
--
ALTER TABLE `nachhilfegesuch`
  ADD CONSTRAINT `nachhilfegesuch_ibfk_1` FOREIGN KEY (`FachID`) REFERENCES `fach` (`FachID`),
  ADD CONSTRAINT `nachhilfegesuch_ibfk_2` FOREIGN KEY (`SchuelerID`) REFERENCES `schueler` (`SchuelerID`) ON DELETE RESTRICT ON UPDATE RESTRICT;

--
-- Constraints for table `schueler`
--
ALTER TABLE `schueler`
  ADD CONSTRAINT `schueler_ibfk_2` FOREIGN KEY (`KlassenID`) REFERENCES `klasse` (`KlassenID`);

--
-- Constraints for table `tutor`
--
ALTER TABLE `tutor`
  ADD CONSTRAINT `tutor_ibfk_2` FOREIGN KEY (`SchuelerID`) REFERENCES `schueler` (`SchuelerID`),
  ADD CONSTRAINT `tutor_ibfk_3` FOREIGN KEY (`LoginID`) REFERENCES `login` (`LoginID`) ON DELETE RESTRICT ON UPDATE RESTRICT;

--
-- Constraints for table `zeitspanne`
--
ALTER TABLE `zeitspanne`
  ADD CONSTRAINT `zeitspanne_ibfk_1` FOREIGN KEY (`WTID`) REFERENCES `wochentage` (`WTID`),
  ADD CONSTRAINT `zeitspanne_ibfk_2` FOREIGN KEY (`SID`) REFERENCES `schueler` (`SchuelerID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
