-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1:3306
-- Généré le : ven. 28 juin 2024 à 07:56
-- Version du serveur : 8.0.31
-- Version de PHP : 8.0.26

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `asp_dev`
--
CREATE DATABASE IF NOT EXISTS `asp_dev` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `asp_dev`;

-- --------------------------------------------------------

--
-- Structure de la table `exportexcel_histo`
--

DROP TABLE IF EXISTS `exportexcel_histo`;
CREATE TABLE IF NOT EXISTS `exportexcel_histo` (
  `id` int NOT NULL AUTO_INCREMENT,
  `dateFrom` date NOT NULL,
  `dateTo` date NOT NULL,
  `dateNow` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `user_maj` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `leaves`
--

DROP TABLE IF EXISTS `leaves`;
CREATE TABLE IF NOT EXISTS `leaves` (
  `leaveId` int NOT NULL AUTO_INCREMENT,
  `reason` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`leaveId`),
  UNIQUE KEY `reason` (`reason`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déclencheurs `leaves`
--
DROP TRIGGER IF EXISTS `trg_leaves_after_delete`;
DELIMITER $$
CREATE TRIGGER `trg_leaves_after_delete` AFTER DELETE ON `leaves` FOR EACH ROW BEGIN
	DECLARE user_maj INT;
    SET user_maj = @userConnected;
    
    INSERT INTO leave_histo (leaveId, old_reason, type_maj, user_maj)
    VALUES (OLD.leaveId, OLD.reason, 'DELETE', user_maj);
END
$$
DELIMITER ;
DROP TRIGGER IF EXISTS `trg_leaves_after_update`;
DELIMITER $$
CREATE TRIGGER `trg_leaves_after_update` AFTER UPDATE ON `leaves` FOR EACH ROW BEGIN
	DECLARE user_maj INT;
    SET user_maj = @userConnected;
    
    INSERT INTO leave_histo (leaveId, old_reason, last_reason, type_maj, user_maj)
    VALUES (OLD.leaveId, OLD.reason, NEW.reason, 'UPDATE', user_maj);
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `leave_histo`
--

DROP TABLE IF EXISTS `leave_histo`;
CREATE TABLE IF NOT EXISTS `leave_histo` (
  `id` int NOT NULL AUTO_INCREMENT,
  `leaveId` int NOT NULL,
  `old_reason` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `last_reason` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `type_maj` varchar(20) COLLATE utf8mb4_general_ci NOT NULL,
  `date_maj` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `user_maj` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `project`
--

DROP TABLE IF EXISTS `project`;
CREATE TABLE IF NOT EXISTS `project` (
  `projectId` int NOT NULL AUTO_INCREMENT,
  `name` varchar(150) COLLATE utf8mb4_general_ci NOT NULL,
  `description` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  PRIMARY KEY (`projectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déclencheurs `project`
--
DROP TRIGGER IF EXISTS `after_project_delete`;
DELIMITER $$
CREATE TRIGGER `after_project_delete` AFTER DELETE ON `project` FOR EACH ROW BEGIN
	DECLARE user_maj INT;
    SET user_maj = @userConnected;
    INSERT INTO project_histo (projectId, old_name, old_description, last_name, last_description, type_maj, user_maj)
    VALUES (OLD.projectId, OLD.name, OLD.description, NULL, NULL, 'DELETE', user_maj);
END
$$
DELIMITER ;
DROP TRIGGER IF EXISTS `after_project_update`;
DELIMITER $$
CREATE TRIGGER `after_project_update` AFTER UPDATE ON `project` FOR EACH ROW BEGIN
	DECLARE user_maj INT;
    SET user_maj = @userConnected;
    INSERT INTO project_histo (projectId, old_name, old_description, last_name, last_description, type_maj, user_maj)
    VALUES (OLD.projectId, OLD.name, OLD.description, NEW.name, NEW.description, 'UPDATE', user_maj);
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `project_histo`
--

DROP TABLE IF EXISTS `project_histo`;
CREATE TABLE IF NOT EXISTS `project_histo` (
  `id` int NOT NULL AUTO_INCREMENT,
  `projectId` int NOT NULL,
  `old_name` varchar(150) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `old_description` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `last_name` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `last_description` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `type_maj` varchar(20) COLLATE utf8mb4_general_ci NOT NULL,
  `date_maj` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `user_maj` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `role`
--

DROP TABLE IF EXISTS `role`;
CREATE TABLE IF NOT EXISTS `role` (
  `roleId` int NOT NULL AUTO_INCREMENT,
  `name` varchar(50) COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`roleId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `tasks`
--

DROP TABLE IF EXISTS `tasks`;
CREATE TABLE IF NOT EXISTS `tasks` (
  `taskId` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
  `projectId` int NOT NULL,
  PRIMARY KEY (`taskId`),
  KEY `task_project_FK` (`projectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déclencheurs `tasks`
--
DROP TRIGGER IF EXISTS `after_task_delete`;
DELIMITER $$
CREATE TRIGGER `after_task_delete` AFTER DELETE ON `tasks` FOR EACH ROW BEGIN
	DECLARE user_maj INT;
    SET user_maj = @userConnected;
    INSERT INTO task_histo (taskId, old_name, last_name, old_projectId, last_projectId, type_maj, user_maj)
    VALUES (OLD.taskId, OLD.name, NULL, OLD.projectId, NULL, 'DELETE', user_maj);
END
$$
DELIMITER ;
DROP TRIGGER IF EXISTS `after_task_update`;
DELIMITER $$
CREATE TRIGGER `after_task_update` AFTER UPDATE ON `tasks` FOR EACH ROW BEGIN
	DECLARE user_maj INT;
    SET user_maj = @userConnected;
    INSERT INTO task_histo (taskId, old_name, last_name, old_projectId, last_projectId, type_maj, user_maj)
    VALUES (OLD.taskId, OLD.name, NEW.name, OLD.projectId, NEW.projectId, 'UPDATE', user_maj);
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `task_histo`
--

DROP TABLE IF EXISTS `task_histo`;
CREATE TABLE IF NOT EXISTS `task_histo` (
  `id` int NOT NULL AUTO_INCREMENT,
  `taskId` int NOT NULL,
  `old_name` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `last_name` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `old_projectId` int DEFAULT NULL,
  `last_projectId` int DEFAULT NULL,
  `type_maj` varchar(20) COLLATE utf8mb4_general_ci NOT NULL,
  `date_maj` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `user_maj` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `user`
--

DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
  `userId` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `surName` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `username` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
  `password` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
  `email` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL,
  PRIMARY KEY (`userId`),
  UNIQUE KEY `username` (`username`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déclencheurs `user`
--
DROP TRIGGER IF EXISTS `trg_user_after_delete`;
DELIMITER $$
CREATE TRIGGER `trg_user_after_delete` AFTER DELETE ON `user` FOR EACH ROW BEGIN
	DECLARE user_maj INT;
    SET user_maj = @userConnected;
    
    INSERT INTO user_histo (UserId, old_name, old_surname, old_email, old_password, old_username, type_maj, user_maj)
    VALUES (OLD.UserId, OLD.name, OLD.surname, OLD.email, OLD.password, OLD.username, 'DELETE', user_maj);
END
$$
DELIMITER ;
DROP TRIGGER IF EXISTS `trg_user_after_update`;
DELIMITER $$
CREATE TRIGGER `trg_user_after_update` AFTER UPDATE ON `user` FOR EACH ROW BEGIN
	DECLARE user_maj INT;
    SET user_maj = @userConnected;

    INSERT INTO user_histo (UserId, old_name, old_surname, old_email, old_password, old_username, last_name, last_surname, last_email, last_password, last_username, type_maj, user_maj)
    VALUES (OLD.UserId, OLD.name, OLD.surname, OLD.email, OLD.password, OLD.username, NEW.name, NEW.surname, NEW.email, NEW.password, NEW.username, 'UPDATE', user_maj);
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `userrole`
--

DROP TABLE IF EXISTS `userrole`;
CREATE TABLE IF NOT EXISTS `userrole` (
  `idUserRole` int NOT NULL AUTO_INCREMENT,
  `roleId` int NOT NULL,
  `userId` int NOT NULL,
  PRIMARY KEY (`idUserRole`),
  KEY `Role ID` (`roleId`),
  KEY `User ID` (`userId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `usertask`
--

DROP TABLE IF EXISTS `usertask`;
CREATE TABLE IF NOT EXISTS `usertask` (
  `UserTaskId` int NOT NULL AUTO_INCREMENT,
  `taskId` int DEFAULT NULL,
  `leaveId` int DEFAULT NULL,
  `isLeave` tinyint(1) NOT NULL,
  `userId` int NOT NULL,
  `date` datetime NOT NULL,
  `hours` double NOT NULL,
  PRIMARY KEY (`UserTaskId`),
  KEY `taskId` (`taskId`),
  KEY `leaveId` (`leaveId`),
  KEY `userId` (`userId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déclencheurs `usertask`
--
DROP TRIGGER IF EXISTS `after_usertask_delete`;
DELIMITER $$
CREATE TRIGGER `after_usertask_delete` AFTER DELETE ON `usertask` FOR EACH ROW BEGIN

   DECLARE user_maj INT;

   SET user_maj = @userConnected;
    
 INSERT into usertask_histo(
	usertask_histo.UserTaskId,
    usertask_histo.old_taskId, 
    usertask_histo.old_leaveId ,   
    usertask_histo.old_userId ,  
    usertask_histo.old_date , 
    usertask_histo.old_hours ,  
    usertask_histo.type_maj,
     usertask_histo.user_maj
) VALUES(
 OLD.UserTaskId,
 OLD.taskId, 
 OLD.leaveId,  
 OLD.userId,
 OLD.date , 
 OLD.hours , 
"DELETE",
 user_maj
);

END
$$
DELIMITER ;
DROP TRIGGER IF EXISTS `after_usertask_update`;
DELIMITER $$
CREATE TRIGGER `after_usertask_update` AFTER UPDATE ON `usertask` FOR EACH ROW begin 

	DECLARE user_maj INT;

    SET user_maj = @userConnected;
    
	 INSERT into  usertask_histo(
    usertask_histo.UserTaskId,
    usertask_histo.old_taskId,
    usertask_histo.last_taskId , 
    usertask_histo.old_leaveId ,  
    usertask_histo.last_leaveId , 
    usertask_histo.old_userId , 
    usertask_histo.last_userId , 
    usertask_histo.old_date , 
    usertask_histo.last_date , 
    usertask_histo.old_hours , 
    usertask_histo.last_hours , 
    usertask_histo.type_maj,
    usertask_histo.user_maj
) VALUES(
 OLD.UserTaskId,
 OLD.taskId,
 NEW.taskId , 
 OLD.leaveId,  
 NEW.leaveId, 
 OLD.userId, 
 NEW.userId, 
 OLD.date , 
 NEW.date , 
 OLD.hours , 
 NEW.hours ,
"UPDATE",
 user_maj
);

END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `usertask_histo`
--

DROP TABLE IF EXISTS `usertask_histo`;
CREATE TABLE IF NOT EXISTS `usertask_histo` (
  `id` int NOT NULL AUTO_INCREMENT,
  `UserTaskId` int NOT NULL,
  `last_taskId` int DEFAULT NULL,
  `old_taskId` int DEFAULT NULL,
  `old_leaveId` int DEFAULT NULL,
  `last_leaveId` int DEFAULT NULL,
  `old_userId` int DEFAULT NULL,
  `last_userId` int DEFAULT NULL,
  `old_date` datetime DEFAULT NULL,
  `last_date` datetime DEFAULT NULL,
  `old_hours` double DEFAULT NULL,
  `last_hours` double DEFAULT NULL,
  `type_maj` varchar(20) COLLATE utf8mb4_general_ci NOT NULL,
  `date_maj` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `user_maj` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `user_histo`
--

DROP TABLE IF EXISTS `user_histo`;
CREATE TABLE IF NOT EXISTS `user_histo` (
  `id` int NOT NULL AUTO_INCREMENT,
  `userId` int NOT NULL,
  `old_name` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `last_name` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `old_surName` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `last_surName` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `old_username` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `last_username` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `old_password` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `last_password` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `old_email` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `last_email` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `type_maj` varchar(20) COLLATE utf8mb4_general_ci NOT NULL,
  `date_maj` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `user_maj` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
