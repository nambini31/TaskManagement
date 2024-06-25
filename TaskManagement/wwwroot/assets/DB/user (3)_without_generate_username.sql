-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1
-- Généré le : mar. 25 juin 2024 à 08:54
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
(20, 'Developper', 'Mada', 'lesgars', '$2a$11$MUMZUq/eR4YgaOiCzfI/aOgUhkIWfgPRQJENLCR66xLulGvHJ1JHO', ''),
(21, 'Nico', 'Tahindraza', 'ntata', '$2a$11$GO9kOycD19oH2LwnFXmsCO.DvPimk3B3CpHLgAo0Hbc3M3Mvh2OOu', 'teste@test.mu');

--
-- Déclencheurs `user`
--
DELIMITER $$
CREATE TRIGGER `trg_user_after_delete` AFTER DELETE ON `user` FOR EACH ROW BEGIN
    INSERT INTO user_histo (UserId, old_name, old_surname, old_email, old_password, old_username, type_maj, user_maj)
    VALUES (OLD.UserId, OLD.name, OLD.surname, OLD.email, OLD.password, OLD.username, 'DELETE', USER());
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_user_after_update` AFTER UPDATE ON `user` FOR EACH ROW BEGIN
	DECLARE user_maj INT;
    SET user_maj = (SELECT user_id FROM app_context ORDER BY context_id DESC LIMIT 1);

    INSERT INTO user_histo (UserId, old_name, old_surname, old_email, old_password, old_username, last_name, last_surname, last_email, last_password, last_username, type_maj, user_maj)
    VALUES (OLD.UserId, OLD.name, OLD.surname, OLD.email, OLD.password, OLD.username, NEW.name, NEW.surname, NEW.email, NEW.password, NEW.username, 'UPDATE', user_maj);
END
$$
DELIMITER ;

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`userId`),
  ADD UNIQUE KEY `username` (`username`) USING BTREE;

--
-- AUTO_INCREMENT pour les tables déchargées
--

--
-- AUTO_INCREMENT pour la table `user`
--
ALTER TABLE `user`
  MODIFY `userId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
