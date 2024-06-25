-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1
-- Généré le : mar. 25 juin 2024 à 11:26
-- Version du serveur : 10.4.32-MariaDB
-- Version de PHP : 8.2.12

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

-- --------------------------------------------------------

--
-- Structure de la table `exportexcel_histo`
--

CREATE TABLE `exportexcel_histo` (
  `id` int(11) NOT NULL,
  `dateFrom` datetime NOT NULL,
  `dateTo` datetime NOT NULL,
  `dateNow` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `leaves`
--

CREATE TABLE `leaves` (
  `leaveId` int(11) NOT NULL,
  `reason` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `leaves`
--

INSERT INTO `leaves` (`leaveId`, `reason`) VALUES
(1, 'Autre leave'),
(2, 'Cyclone'),
(7, 'fdfdf'),
(3, 'Innondation'),
(5, 'Local');

--
-- Déclencheurs `leaves`
--
DELIMITER $$
CREATE TRIGGER `trg_leaves_after_delete` AFTER DELETE ON `leaves` FOR EACH ROW BEGIN
	DECLARE user_maj INT;
    SET user_maj = @userConnected;
    
    INSERT INTO leave_histo (leaveId, old_reason, type_maj, user_maj)
    VALUES (OLD.leaveId, OLD.reason, 'DELETE', user_maj);
END
$$
DELIMITER ;
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

CREATE TABLE `leave_histo` (
  `id` int(11) NOT NULL,
  `leaveId` int(11) NOT NULL,
  `old_reason` varchar(255) DEFAULT NULL,
  `last_reason` varchar(255) DEFAULT NULL,
  `type_maj` varchar(20) NOT NULL,
  `date_maj` timestamp NOT NULL DEFAULT current_timestamp(),
  `user_maj` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `leave_histo`
--

INSERT INTO `leave_histo` (`id`, `leaveId`, `old_reason`, `last_reason`, `type_maj`, `date_maj`, `user_maj`) VALUES
(1, 2, 'Cyclone', 'Cyclone ( inondation )', 'UPDATE', '2024-06-18 15:51:17', 0),
(2, 1, 'Sick', NULL, 'DELETE', '2024-06-23 18:53:37', 0),
(3, 2, 'Cyclone ( inondation )', NULL, 'DELETE', '2024-06-23 18:54:36', 0),
(4, 3, 'gfgfgf', 'gfgfgf dsdsd', 'UPDATE', '2024-06-23 19:23:02', 0),
(5, 4, 'gfgg', 'gfgg dqsdqdqs', 'UPDATE', '2024-06-23 19:23:21', 0),
(6, 4, 'gfgg dqsdqdqs', 'ddddd', 'UPDATE', '2024-06-23 19:23:26', 0),
(7, 4, 'ddddd', 'ddddd', 'UPDATE', '2024-06-23 19:29:17', 0),
(8, 6, 'fdfdf', 'fdfdf', 'UPDATE', '2024-06-23 19:29:24', 0),
(9, 1, 'ddddd', 'reason retoa', 'UPDATE', '2024-06-23 19:48:39', 0),
(10, 2, 'fdfdf', 'Cyclone', 'UPDATE', '2024-06-24 11:07:53', 0),
(11, 5, 'fhf', 'Sick', 'UPDATE', '2024-06-24 11:07:59', 0),
(12, 3, 'gfgfgf dsdsd', 'Innondation', 'UPDATE', '2024-06-24 11:08:06', 0),
(13, 1, 'reason retoa', 'Autre leave', 'UPDATE', '2024-06-24 11:08:39', 0),
(14, 5, 'Sick', 'Local', 'UPDATE', '2024-06-24 11:11:13', 0);

-- --------------------------------------------------------

--
-- Structure de la table `project`
--

CREATE TABLE `project` (
  `projectId` int(11) NOT NULL,
  `name` varchar(150) NOT NULL,
  `description` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `project`
--

INSERT INTO `project` (`projectId`, `name`, `description`) VALUES
(1, 'GesPro', 'gestion d\'article , categorie , commande'),
(2, 'Chating', 'appel video , appel without video , kkkk');

--
-- Déclencheurs `project`
--
DELIMITER $$
CREATE TRIGGER `after_project_delete` AFTER DELETE ON `project` FOR EACH ROW BEGIN
	DECLARE user_maj INT;
    SET user_maj = @userConnected;
    INSERT INTO project_histo (projectId, old_name, old_description, last_name, last_description, type_maj, user_maj)
    VALUES (OLD.projectId, OLD.name, OLD.description, NULL, NULL, 'DELETE', user_maj);
END
$$
DELIMITER ;
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

CREATE TABLE `project_histo` (
  `id` int(11) NOT NULL,
  `projectId` int(11) NOT NULL,
  `old_name` varchar(150) DEFAULT NULL,
  `old_description` varchar(255) DEFAULT NULL,
  `last_name` varchar(255) DEFAULT NULL,
  `last_description` varchar(255) DEFAULT NULL,
  `type_maj` varchar(20) NOT NULL,
  `date_maj` timestamp NOT NULL DEFAULT current_timestamp(),
  `user_maj` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `project_histo`
--

INSERT INTO `project_histo` (`id`, `projectId`, `old_name`, `old_description`, `last_name`, `last_description`, `type_maj`, `date_maj`, `user_maj`) VALUES
(1, 1, 'E-Commerce', 'gestion d\'article , categorie', 'E-Commerce', 'gestion d\'article , categorie , commande', 'UPDATE', '2024-06-18 15:53:56', 14),
(2, 2, 'Chating', 'appel video , appel without video', 'Chating', 'appel video , appel without video , kkkk', 'UPDATE', '2024-06-18 15:54:30', 14),
(3, 3, 'gfgf', 'gfgfgf', NULL, NULL, 'DELETE', '2024-06-18 15:55:02', 111),
(4, 1, 'E-Commerce', 'gestion d\'article , categorie , commande', 'GesPro', 'gestion d\'article , categorie , commande', 'UPDATE', '2024-06-24 11:13:30', 14);

-- --------------------------------------------------------

--
-- Structure de la table `role`
--

CREATE TABLE `role` (
  `roleId` int(11) NOT NULL,
  `name` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `role`
--

INSERT INTO `role` (`roleId`, `name`) VALUES
(1, 'Admin'),
(2, 'User');

-- --------------------------------------------------------

--
-- Structure de la table `tasks`
--

CREATE TABLE `tasks` (
  `taskId` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `projectId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `tasks`
--

INSERT INTO `tasks` (`taskId`, `name`, `projectId`) VALUES
(1, 'Login et register', 1),
(2, 'Crud leaves et projects', 1),
(3, 'Authentification user', 1),
(4, 'Crud saisie temps', 1);

--
-- Déclencheurs `tasks`
--
DELIMITER $$
CREATE TRIGGER `after_task_delete` AFTER DELETE ON `tasks` FOR EACH ROW BEGIN
	DECLARE user_maj INT;
    SET user_maj = @userConnected;
    INSERT INTO task_histo (taskId, old_name, last_name, old_projectId, last_projectId, type_maj, user_maj)
    VALUES (OLD.taskId, OLD.name, NULL, OLD.projectId, NULL, 'DELETE', user_maj);
END
$$
DELIMITER ;
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

CREATE TABLE `task_histo` (
  `id` int(11) NOT NULL,
  `taskId` int(11) NOT NULL,
  `old_name` varchar(255) DEFAULT NULL,
  `last_name` varchar(255) DEFAULT NULL,
  `old_projectId` int(11) DEFAULT NULL,
  `last_projectId` int(11) DEFAULT NULL,
  `type_maj` varchar(20) NOT NULL,
  `date_maj` timestamp NOT NULL DEFAULT current_timestamp(),
  `user_maj` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `task_histo`
--

INSERT INTO `task_histo` (`id`, `taskId`, `old_name`, `last_name`, `old_projectId`, `last_projectId`, `type_maj`, `date_maj`, `user_maj`) VALUES
(1, 1, 'fdfdfd', ' gfgdgdf fg gudfogdoifug dfufoigu dfgiduf guoifug oidfug fogudf ogufdgoifd', 1, 1, 'UPDATE', '2024-06-20 22:50:56', 787),
(2, 1, ' gfgdgdf fg gudfogdoifug dfufoigu dfgiduf guoifug oidfug fogudf ogufdgoifd', 'Login et register', 1, 1, 'UPDATE', '2024-06-24 11:11:39', 787),
(3, 2, 'hfhgfhgfhgfhhgf', 'CRUD leaves et projects', 1, 1, 'UPDATE', '2024-06-24 11:11:55', 787),
(4, 3, 'nico is rae', 'Authentification user', 1, 1, 'UPDATE', '2024-06-24 11:12:26', 787),
(5, 4, 'raiainnanna', '', 1, 1, 'UPDATE', '2024-06-24 11:12:33', 787),
(6, 4, '', 'Crud saisie temps', 1, 1, 'UPDATE', '2024-06-24 11:12:50', 787),
(7, 2, 'CRUD leaves et projects', 'Crud leaves et projects', 1, 1, 'UPDATE', '2024-06-24 11:12:58', 787);

-- --------------------------------------------------------

--
-- Structure de la table `user`
--

CREATE TABLE `user` (
  `userId` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `surName` varchar(255) DEFAULT NULL,
  `username` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `email` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `user`
--

INSERT INTO `user` (`userId`, `name`, `surName`, `username`, `password`, `email`) VALUES
(1, 'Developper', 'Mada', 'lesgars', '$2a$11$eXjTlMDh7pR6vHftageQReJqYOHPTmjMmaI1MB3HoqylroralBL7W', ''),
(2, 'Nico', 'Tahindraza', 'NicoSaim', '$2a$11$FyC29KsBPanUG1qxctXXh.UikvXnjJxtmBW0Po4zefUzFxGQEJS36', 'nico@gmail.com'),
(3, 'Georges', 'Tolojanahry', 'Georges', '$2a$11$mxPxrIjt6zd6bo.mJ8wTQuYZPvAf9ABnbvjWNreWHds6U61UiYgmy', 'georges@gmail.com');

--
-- Déclencheurs `user`
--
DELIMITER $$
CREATE TRIGGER `trg_user_after_delete` AFTER DELETE ON `user` FOR EACH ROW BEGIN
	DECLARE user_maj INT;
    SET user_maj = @userConnected;
    
    INSERT INTO user_histo (UserId, old_name, old_surname, old_email, old_password, old_username, type_maj, user_maj)
    VALUES (OLD.UserId, OLD.name, OLD.surname, OLD.email, OLD.password, OLD.username, 'DELETE', user_maj);
END
$$
DELIMITER ;
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

CREATE TABLE `userrole` (
  `idUserRole` int(11) NOT NULL,
  `roleId` int(11) NOT NULL,
  `userId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `userrole`
--

INSERT INTO `userrole` (`idUserRole`, `roleId`, `userId`) VALUES
(1, 1, 1),
(2, 2, 2),
(3, 2, 3);

-- --------------------------------------------------------

--
-- Structure de la table `usertask`
--

CREATE TABLE `usertask` (
  `UserTaskId` int(11) NOT NULL,
  `taskId` int(11) DEFAULT NULL,
  `leaveId` int(11) DEFAULT NULL,
  `userId` int(11) NOT NULL,
  `date` datetime NOT NULL,
  `hours` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `usertask`
--

INSERT INTO `usertask` (`UserTaskId`, `taskId`, `leaveId`, `userId`, `date`, `hours`) VALUES
(1, NULL, 1, 3, '2024-06-18 15:56:41', 5),
(2, 3, NULL, 2, '2024-06-18 15:56:41', 4),
(17, NULL, 2, 2, '2024-06-18 15:56:41', 4),
(18, 2, NULL, 3, '2024-06-19 22:00:29', 5.3),
(32, 2, NULL, 2, '2024-06-18 15:56:41', 5.5);

--
-- Déclencheurs `usertask`
--
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

CREATE TABLE `usertask_histo` (
  `id` int(11) NOT NULL,
  `UserTaskId` int(11) NOT NULL,
  `last_taskId` int(11) DEFAULT NULL,
  `old_taskId` int(11) DEFAULT NULL,
  `old_leaveId` int(11) DEFAULT NULL,
  `last_leaveId` int(11) DEFAULT NULL,
  `old_userId` int(11) DEFAULT NULL,
  `last_userId` int(11) DEFAULT NULL,
  `old_date` datetime DEFAULT NULL,
  `last_date` datetime DEFAULT NULL,
  `old_hours` double DEFAULT NULL,
  `last_hours` double DEFAULT NULL,
  `type_maj` varchar(20) NOT NULL,
  `date_maj` datetime NOT NULL DEFAULT current_timestamp(),
  `user_maj` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `usertask_histo`
--

INSERT INTO `usertask_histo` (`id`, `UserTaskId`, `last_taskId`, `old_taskId`, `old_leaveId`, `last_leaveId`, `old_userId`, `last_userId`, `old_date`, `last_date`, `old_hours`, `last_hours`, `type_maj`, `date_maj`, `user_maj`) VALUES
(1, 3, NULL, 1, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-18 18:57:11', NULL),
(2, 2, 1, 1, 1, 2222, 1, 444, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-18 18:57:18', NULL),
(3, 2, 1, 1, 2222, 1, 444, 444, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-20 14:19:36', NULL),
(4, 2, 2, 1, 1, 1, 444, 444, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-20 14:19:40', NULL),
(5, 4, 3, 1, 1, 1, 1, 1, '2024-06-20 11:19:10', '2024-06-20 11:19:10', 5, 5, 'UPDATE', '2024-06-20 14:19:43', NULL),
(6, 2, 2, 2, 1, 1, 444, 2, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-22 18:35:37', NULL),
(7, 2, 2, 2, 1, 1, 2, 3, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-22 18:35:49', NULL),
(8, 2, 2, 2, 1, 1, 3, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-22 18:35:52', NULL),
(9, 4, NULL, 3, 1, NULL, 1, NULL, '2024-06-20 11:19:10', NULL, 5, NULL, 'DELETE', '2024-06-22 18:53:12', NULL),
(10, 5, NULL, 2, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-22 20:51:32', NULL),
(11, 8, NULL, 2, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-22 20:51:59', NULL),
(12, 12, NULL, 2, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-22 20:54:22', NULL),
(13, 11, NULL, 2, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-22 20:54:27', NULL),
(14, 10, NULL, 2, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-22 20:54:30', NULL),
(15, 9, NULL, 2, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-22 21:17:54', NULL),
(16, 7, NULL, 2, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-22 21:18:27', NULL),
(17, 15, NULL, 2, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-22 21:29:40', NULL),
(18, 14, NULL, 2, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-22 21:32:38', NULL),
(19, 13, NULL, 1, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-22 21:33:03', NULL),
(20, 6, NULL, 2, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-22 21:33:12', NULL),
(21, 18, 1, NULL, NULL, NULL, 0, 0, '0000-00-00 00:00:00', '0000-00-00 00:00:00', 0, 0, 'UPDATE', '2024-06-22 21:56:11', NULL),
(22, 21, 3, NULL, NULL, NULL, 0, 0, '0000-00-00 00:00:00', '0000-00-00 00:00:00', 0, 0, 'UPDATE', '2024-06-22 21:56:16', NULL),
(23, 21, 3, 3, NULL, 1, 0, 0, '0000-00-00 00:00:00', '0000-00-00 00:00:00', 0, 0, 'UPDATE', '2024-06-22 21:56:20', NULL),
(24, 18, 1, 1, NULL, 1, 0, 0, '0000-00-00 00:00:00', '0000-00-00 00:00:00', 0, 0, 'UPDATE', '2024-06-22 21:56:23', NULL),
(25, 23, NULL, 2, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-22 21:57:36', NULL),
(26, 20, NULL, 2, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-22 21:57:50', NULL),
(27, 18, 1, 1, 1, 1, 0, 1, '0000-00-00 00:00:00', '0000-00-00 00:00:00', 0, 0, 'UPDATE', '2024-06-22 21:58:22', NULL),
(28, 24, 2, NULL, NULL, NULL, 0, 0, '0000-00-00 00:00:00', '0000-00-00 00:00:00', 0, 0, 'UPDATE', '2024-06-22 21:58:28', NULL),
(29, 24, 2, 2, NULL, 1, 0, 0, '0000-00-00 00:00:00', '0000-00-00 00:00:00', 0, 0, 'UPDATE', '2024-06-22 21:58:31', NULL),
(30, 24, 2, 2, 1, 1, 0, 1, '0000-00-00 00:00:00', '0000-00-00 00:00:00', 0, 0, 'UPDATE', '2024-06-22 21:58:34', NULL),
(31, 18, 1, 1, 1, 1, 1, 1, '0000-00-00 00:00:00', '2024-06-19 22:00:29', 0, 0, 'UPDATE', '2024-06-22 22:00:34', NULL),
(32, 21, 3, 3, 1, 1, 0, 0, '0000-00-00 00:00:00', '2024-06-21 00:00:00', 0, 0, 'UPDATE', '2024-06-22 22:00:44', NULL),
(33, 24, 2, 2, 1, 1, 1, 1, '0000-00-00 00:00:00', '1900-01-19 00:00:00', 0, 0, 'UPDATE', '2024-06-22 22:00:50', NULL),
(34, 21, NULL, 3, 1, NULL, 0, NULL, '2024-06-21 00:00:00', NULL, 0, NULL, 'DELETE', '2024-06-22 22:01:08', NULL),
(35, 24, 2, 2, 1, 1, 1, 1, '1900-01-19 00:00:00', '1900-01-18 00:00:00', 0, 0, 'UPDATE', '2024-06-22 22:01:39', NULL),
(36, 24, NULL, 2, 1, NULL, 1, NULL, '1900-01-18 00:00:00', NULL, 0, NULL, 'DELETE', '2024-06-22 22:01:43', NULL),
(37, 30, NULL, 1, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-22 22:04:01', NULL),
(38, 19, NULL, 1, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-22 22:13:21', NULL),
(39, 28, NULL, 1, 1, NULL, 1, NULL, '2024-06-19 22:00:29', NULL, 0, NULL, 'DELETE', '2024-06-22 22:13:27', NULL),
(40, 29, NULL, 1, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-23 01:46:13', NULL),
(41, 26, NULL, 1, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-23 01:46:20', NULL),
(42, 27, NULL, 1, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-23 01:46:23', NULL),
(43, 25, NULL, 1, 1, NULL, 1, NULL, '2024-06-19 22:00:29', NULL, 0, NULL, 'DELETE', '2024-06-23 01:46:28', NULL),
(44, 22, NULL, 1, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-23 01:46:32', NULL),
(45, 16, NULL, 1, 1, NULL, 1, NULL, '2024-06-18 15:56:41', NULL, 4, NULL, 'DELETE', '2024-06-23 17:01:45', NULL),
(46, 17, 2, 2, 1, 0, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-23 22:35:06', NULL),
(47, 17, 2, 2, 0, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-23 22:59:19', NULL),
(48, 2, 0, 2, 1, 1, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-23 23:35:22', NULL),
(49, 1, 0, 1, 1, 1, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-23 23:35:30', NULL),
(50, 18, 0, 1, 1, 1, 1, 1, '2024-06-19 22:00:29', '2024-06-19 22:00:29', 0, 0, 'UPDATE', '2024-06-23 23:35:35', NULL),
(51, 1, NULL, 0, 1, 1, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-24 00:03:44', NULL),
(52, 2, NULL, 0, 1, 1, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-24 00:03:48', NULL),
(53, 18, NULL, 0, 1, 1, 1, 1, '2024-06-19 22:00:29', '2024-06-19 22:00:29', 0, 0, 'UPDATE', '2024-06-24 00:03:52', NULL),
(54, 31, 1, 1, NULL, NULL, 1, 0, '2024-06-19 22:00:29', '2024-06-19 22:00:29', 0, 2, 'UPDATE', '2024-06-24 04:59:58', NULL),
(55, 18, 2, NULL, 1, NULL, 1, 0, '2024-06-19 22:00:29', '2024-06-19 22:00:29', 0, 0, 'UPDATE', '2024-06-24 05:01:36', NULL),
(56, 18, 2, 2, NULL, NULL, 0, 0, '2024-06-19 22:00:29', '2024-06-19 22:00:29', 0, 0, 'UPDATE', '2024-06-24 05:02:07', NULL),
(57, 18, 2, 2, NULL, NULL, 0, 1, '2024-06-19 22:00:29', '2024-06-19 22:00:29', 0, 0, 'UPDATE', '2024-06-24 05:02:10', NULL),
(58, 31, 1, 1, NULL, NULL, 0, 1, '2024-06-19 22:00:29', '2024-06-19 22:00:29', 2, 2, 'UPDATE', '2024-06-24 05:02:17', NULL),
(59, 2, NULL, NULL, 1, 1, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-24 05:13:09', NULL),
(60, 18, 3, 2, NULL, NULL, 1, 1, '2024-06-19 22:00:29', '2024-06-19 22:00:29', 0, 5, 'UPDATE', '2024-06-24 05:13:21', NULL),
(61, 2, 3, NULL, 1, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-24 10:38:04', NULL),
(62, 17, NULL, 2, NULL, 3, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-24 10:38:56', NULL),
(63, 17, 2, NULL, 3, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-24 10:57:56', NULL),
(64, 17, NULL, 2, NULL, 5, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-24 10:58:07', NULL),
(65, 31, NULL, 1, NULL, 2, 1, 1, '2024-06-19 22:00:29', '2024-06-19 22:00:29', 2, 2, 'UPDATE', '2024-06-24 10:58:22', NULL),
(66, 31, 2, NULL, 2, NULL, 1, 1, '2024-06-19 22:00:29', '2024-06-19 22:00:29', 2, 8, 'UPDATE', '2024-06-24 10:58:39', NULL),
(67, 18, 3, 3, NULL, NULL, 1, 3, '2024-06-19 22:00:29', '2024-06-19 22:00:29', 5, 5, 'UPDATE', '2024-06-24 11:03:59', NULL),
(68, 1, NULL, NULL, 1, 1, 1, 3, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-24 11:04:04', NULL),
(69, 17, NULL, NULL, 5, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-24 11:05:01', NULL),
(70, 17, NULL, NULL, NULL, 5, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-24 11:05:12', NULL),
(71, 18, NULL, 3, NULL, 5, 3, 1, '2024-06-19 22:00:29', '2024-06-19 22:00:29', 5, 5, 'UPDATE', '2024-06-24 11:07:27', NULL),
(72, 1, 2, NULL, 1, NULL, 3, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 5, 'UPDATE', '2024-06-24 11:09:19', NULL),
(73, 1, NULL, 2, NULL, 1, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 5, 5, 'UPDATE', '2024-06-24 11:11:31', NULL),
(74, 18, 3, NULL, 5, NULL, 1, 1, '2024-06-19 22:00:29', '2024-06-19 22:00:29', 5, 5, 'UPDATE', '2024-06-24 11:11:38', NULL),
(75, 31, NULL, 2, NULL, NULL, 1, NULL, '2024-06-19 22:00:29', NULL, 8, NULL, 'DELETE', '2024-06-24 13:09:33', NULL),
(76, 18, 1, 3, NULL, NULL, 1, 1, '2024-06-19 22:00:29', '2024-06-19 22:00:29', 5, 5, 'UPDATE', '2024-06-24 13:09:48', NULL),
(77, 32, NULL, NULL, 5, 5, 1, 3, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-24 13:41:46', NULL),
(78, 33, 1, 2, NULL, NULL, 3, 3, '2024-06-19 22:00:29', '2024-06-19 22:00:29', 8, 8, 'UPDATE', '2024-06-24 13:42:02', NULL),
(79, 33, NULL, 1, NULL, NULL, 3, NULL, '2024-06-19 22:00:29', NULL, 8, NULL, 'DELETE', '2024-06-24 13:56:32', NULL),
(80, 32, NULL, NULL, 5, 2, 3, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-24 14:13:49', NULL),
(81, 17, NULL, NULL, 5, 5, 1, 3, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-24 14:22:59', NULL),
(82, 1, NULL, NULL, 1, 1, 1, 3, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 5, 5, 'UPDATE', '2024-06-24 14:23:02', NULL),
(83, 32, 2, NULL, 2, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 5, 'UPDATE', '2024-06-24 14:29:46', NULL),
(84, 2, 3, 3, NULL, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-24 16:19:04', NULL),
(85, 32, 2, 2, NULL, NULL, 1, 4, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 5, 5, 'UPDATE', '2024-06-24 20:05:59', NULL),
(86, 32, 1, 2, NULL, NULL, 4, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 5, 5, 'UPDATE', '2024-06-24 20:47:03', NULL),
(87, 32, 4, 1, NULL, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 5, 6, 'UPDATE', '2024-06-24 20:47:35', NULL),
(88, 17, NULL, NULL, 5, 2, 3, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-24 20:48:04', NULL),
(89, 32, 2, 4, NULL, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 6, 6, 'UPDATE', '2024-06-24 20:49:46', NULL),
(90, 32, 2, 2, NULL, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 6, 0, 'UPDATE', '2024-06-24 22:00:19', NULL),
(91, 32, 2, 2, NULL, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 0, 0, 'UPDATE', '2024-06-24 22:00:40', NULL),
(92, 32, 2, 2, NULL, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 0, 2, 'UPDATE', '2024-06-24 22:00:49', NULL),
(93, 32, 2, 2, NULL, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 2, 0, 'UPDATE', '2024-06-24 22:05:42', NULL),
(94, 32, 2, 2, NULL, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 0, 0, 'UPDATE', '2024-06-24 22:05:43', NULL),
(95, 32, 2, 2, NULL, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 0, 0, 'UPDATE', '2024-06-24 22:10:31', NULL),
(96, 32, 2, 2, NULL, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 0, 2.5, 'UPDATE', '2024-06-24 22:11:39', NULL),
(97, 32, 2, 2, NULL, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 2.5, 2.3, 'UPDATE', '2024-06-24 22:48:32', NULL),
(98, 32, 2, 2, NULL, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 2.3, 2.8, 'UPDATE', '2024-06-24 22:49:02', NULL),
(99, 32, 2, 2, NULL, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 2.8, 2.8, 'UPDATE', '2024-06-24 22:54:10', NULL),
(100, 32, 2, 2, NULL, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 2.8, 5.2, 'UPDATE', '2024-06-24 22:54:35', NULL),
(101, 32, 2, 2, NULL, NULL, 1, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 5.2, 5.5, 'UPDATE', '2024-06-24 23:28:20', NULL),
(102, 18, 1, 1, NULL, NULL, 1, 1, '2024-06-19 22:00:29', '2024-06-19 22:00:29', 5, 5.3, 'UPDATE', '2024-06-24 23:28:30', NULL),
(103, 32, 2, 2, NULL, NULL, 1, 3, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 5.5, 5.5, 'UPDATE', '2024-06-25 00:57:50', NULL),
(104, 32, 2, 2, NULL, NULL, 3, 1, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 5.5, 5.5, 'UPDATE', '2024-06-25 00:57:58', NULL),
(105, 18, 1, 1, NULL, NULL, 1, 3, '2024-06-19 22:00:29', '2024-06-19 22:00:29', 5.3, 5.3, 'UPDATE', '2024-06-25 00:58:27', NULL),
(106, 18, 2, 1, NULL, NULL, 3, 3, '2024-06-19 22:00:29', '2024-06-19 22:00:29', 5.3, 5.3, 'UPDATE', '2024-06-25 00:58:30', NULL),
(107, 2, 3, 3, NULL, NULL, 1, 2, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-25 10:15:12', NULL),
(108, 17, NULL, NULL, 2, 2, 1, 2, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 4, 4, 'UPDATE', '2024-06-25 10:15:14', NULL),
(109, 32, 2, 2, NULL, NULL, 1, 2, '2024-06-18 15:56:41', '2024-06-18 15:56:41', 5.5, 5.5, 'UPDATE', '2024-06-25 10:15:17', NULL);

-- --------------------------------------------------------

--
-- Structure de la table `user_histo`
--

CREATE TABLE `user_histo` (
  `id` int(11) NOT NULL,
  `userId` int(11) NOT NULL,
  `old_name` varchar(255) DEFAULT NULL,
  `last_name` varchar(255) DEFAULT NULL,
  `old_surName` varchar(255) DEFAULT NULL,
  `last_surName` varchar(255) DEFAULT NULL,
  `old_username` varchar(255) DEFAULT NULL,
  `last_username` varchar(255) DEFAULT NULL,
  `old_password` varchar(255) DEFAULT NULL,
  `last_password` varchar(255) DEFAULT NULL,
  `old_email` varchar(50) DEFAULT NULL,
  `last_email` varchar(255) DEFAULT NULL,
  `type_maj` varchar(20) NOT NULL,
  `date_maj` datetime NOT NULL DEFAULT current_timestamp(),
  `user_maj` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `user_histo`
--

INSERT INTO `user_histo` (`id`, `userId`, `old_name`, `last_name`, `old_surName`, `last_surName`, `old_username`, `last_username`, `old_password`, `last_password`, `old_email`, `last_email`, `type_maj`, `date_maj`, `user_maj`) VALUES
(1, 2, 'nico', 'nico', 'tahindraza', 'tahindraza gfgfgfg', 'ntahindraza1', 'ntahindraza1', 'dsds', 'dsds', 'dsd', 'dsd', 'UPDATE', '2024-06-18 18:59:11', 0),
(2, 2, 'nico', NULL, 'tahindraza gfgfgfg', NULL, 'ntahindraza1', NULL, 'dsds', NULL, 'dsd', NULL, 'DELETE', '2024-06-18 18:59:14', 0),
(3, 1, 'nico', 'nico', 'tahindraza', 'tahindraza', 'ntahindraza', 'Nico', 'dddddddd', 'dddddddd', 'ddddddd', 'ddddddd', 'UPDATE', '2024-06-24 20:06:17', 0),
(4, 3, 'Jean pierre', 'Jean pierre', 'lahiniriko', 'lahiniriko', 'Jlahiniriko', 'Georges', 'dfdfdfdfdfd', 'dfdfdfdfdfd', 'fdfdfdfd', 'fdfdfdfd', 'UPDATE', '2024-06-24 20:06:25', 0),
(5, 4, 'lahimakoa', 'lahimakoa', 'sahimona', 'sahimona', 'lsahimona', 'JeanPierre', 'ffdffdfdfd', 'ffdffdfdfd', 'fdfdf', 'fdfdf', 'UPDATE', '2024-06-24 20:06:32', 0);

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `exportexcel_histo`
--
ALTER TABLE `exportexcel_histo`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `leaves`
--
ALTER TABLE `leaves`
  ADD PRIMARY KEY (`leaveId`),
  ADD UNIQUE KEY `reason` (`reason`);

--
-- Index pour la table `leave_histo`
--
ALTER TABLE `leave_histo`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `project`
--
ALTER TABLE `project`
  ADD PRIMARY KEY (`projectId`);

--
-- Index pour la table `project_histo`
--
ALTER TABLE `project_histo`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `role`
--
ALTER TABLE `role`
  ADD PRIMARY KEY (`roleId`);

--
-- Index pour la table `tasks`
--
ALTER TABLE `tasks`
  ADD PRIMARY KEY (`taskId`),
  ADD KEY `task_project_FK` (`projectId`);

--
-- Index pour la table `task_histo`
--
ALTER TABLE `task_histo`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`userId`),
  ADD UNIQUE KEY `username` (`username`) USING BTREE;

--
-- Index pour la table `userrole`
--
ALTER TABLE `userrole`
  ADD PRIMARY KEY (`idUserRole`),
  ADD KEY `Role ID` (`roleId`),
  ADD KEY `User ID` (`userId`);

--
-- Index pour la table `usertask`
--
ALTER TABLE `usertask`
  ADD PRIMARY KEY (`UserTaskId`),
  ADD KEY `taskId` (`taskId`),
  ADD KEY `leaveId` (`leaveId`),
  ADD KEY `userId` (`userId`);

--
-- Index pour la table `usertask_histo`
--
ALTER TABLE `usertask_histo`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `user_histo`
--
ALTER TABLE `user_histo`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT pour les tables déchargées
--

--
-- AUTO_INCREMENT pour la table `exportexcel_histo`
--
ALTER TABLE `exportexcel_histo`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `leaves`
--
ALTER TABLE `leaves`
  MODIFY `leaveId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT pour la table `leave_histo`
--
ALTER TABLE `leave_histo`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT pour la table `project`
--
ALTER TABLE `project`
  MODIFY `projectId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT pour la table `project_histo`
--
ALTER TABLE `project_histo`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT pour la table `role`
--
ALTER TABLE `role`
  MODIFY `roleId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT pour la table `tasks`
--
ALTER TABLE `tasks`
  MODIFY `taskId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT pour la table `task_histo`
--
ALTER TABLE `task_histo`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT pour la table `user`
--
ALTER TABLE `user`
  MODIFY `userId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT pour la table `userrole`
--
ALTER TABLE `userrole`
  MODIFY `idUserRole` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT pour la table `usertask`
--
ALTER TABLE `usertask`
  MODIFY `UserTaskId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=34;

--
-- AUTO_INCREMENT pour la table `usertask_histo`
--
ALTER TABLE `usertask_histo`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=110;

--
-- AUTO_INCREMENT pour la table `user_histo`
--
ALTER TABLE `user_histo`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
