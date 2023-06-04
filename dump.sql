-- MySQL dump 10.13  Distrib 8.0.33, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: flowmodel
-- ------------------------------------------------------
-- Server version	8.0.33

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `flowmodel`
--

/*!40000 DROP DATABASE IF EXISTS `flowmodel`*/;

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `flowmodel` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `flowmodel`;

--
-- Table structure for table `category`
--

DROP TABLE IF EXISTS `category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `category` (
  `cat_id` int NOT NULL AUTO_INCREMENT,
  `user_cat` varchar(10) NOT NULL,
  PRIMARY KEY (`cat_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `category`
--

LOCK TABLES `category` WRITE;
/*!40000 ALTER TABLE `category` DISABLE KEYS */;
INSERT INTO `category` VALUES (1,'admin'),(2,'default');
/*!40000 ALTER TABLE `category` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mat_coef`
--

DROP TABLE IF EXISTS `mat_coef`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mat_coef` (
  `mat_id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(60) NOT NULL,
  `category_cat_id` int NOT NULL DEFAULT '1',
  `unit` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`mat_id`),
  KEY `fk_mat_coef_category1_idx` (`category_cat_id`),
  KEY `fk_mat_coef_unit_idx` (`unit`),
  CONSTRAINT `fk_mat_coef_category1` FOREIGN KEY (`category_cat_id`) REFERENCES `category` (`cat_id`),
  CONSTRAINT `fk_mat_coef_unit` FOREIGN KEY (`unit`) REFERENCES `unit` (`unit_title`) ON DELETE SET NULL ON UPDATE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mat_coef`
--

LOCK TABLES `mat_coef` WRITE;
/*!40000 ALTER TABLE `mat_coef` DISABLE KEYS */;
INSERT INTO `mat_coef` VALUES (1,'Индекс течения',1,NULL),(2,'Коэффициент теплоотдачи от крышки',1,'Вт/(м^2*°С)'),(3,'Температура приведения',1,'°С'),(4,'Коэффициент консистенции при температуре приведения',1,'Па*с^n'),(5,'Энергия активации вязкого течения материала',1,'Дж/моль');
/*!40000 ALTER TABLE `mat_coef` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mat_model`
--

DROP TABLE IF EXISTS `mat_model`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mat_model` (
  `mat_model_id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(35) NOT NULL,
  PRIMARY KEY (`mat_model_id`),
  UNIQUE KEY `title_UNIQUE` (`title`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mat_model`
--

LOCK TABLES `mat_model` WRITE;
/*!40000 ALTER TABLE `mat_model` DISABLE KEYS */;
INSERT INTO `mat_model` VALUES (29,'123'),(30,'dfghj'),(31,'qwertyu'),(1,'Первый'),(28,'Тестовый');
/*!40000 ALTER TABLE `mat_model` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mat_set`
--

DROP TABLE IF EXISTS `mat_set`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mat_set` (
  `mat_coef_id` int NOT NULL,
  `mat_model_id` int NOT NULL,
  `value` double DEFAULT NULL,
  PRIMARY KEY (`mat_coef_id`,`mat_model_id`),
  KEY `fk_mat_coef_has_mat_model_mat_model1_idx` (`mat_model_id`),
  KEY `fk_mat_coef_has_mat_model_mat_coef1_idx` (`mat_coef_id`),
  CONSTRAINT `fk_mat_coef_has_mat_model_mat_coef1` FOREIGN KEY (`mat_coef_id`) REFERENCES `mat_coef` (`mat_id`),
  CONSTRAINT `fk_mat_coef_has_mat_model_mat_model1` FOREIGN KEY (`mat_model_id`) REFERENCES `mat_model` (`mat_model_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mat_set`
--

LOCK TABLES `mat_set` WRITE;
/*!40000 ALTER TABLE `mat_set` DISABLE KEYS */;
INSERT INTO `mat_set` VALUES (1,1,0.35),(1,28,3978654322),(2,1,250),(2,28,987654321),(2,29,23456),(2,30,345),(3,1,120),(3,28,97),(4,1,50000),(4,28,19),(4,31,234567),(5,1,48000),(5,28,1337);
/*!40000 ALTER TABLE `mat_set` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `material`
--

DROP TABLE IF EXISTS `material`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `material` (
  `material_id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(35) NOT NULL,
  `category_cat_id` int NOT NULL DEFAULT '1',
  `material_model` int NOT NULL,
  PRIMARY KEY (`material_id`),
  UNIQUE KEY `title_UNIQUE` (`title`),
  UNIQUE KEY `material_model_UNIQUE` (`material_model`),
  KEY `fk_material_category1_idx` (`category_cat_id`),
  CONSTRAINT `fk_material_category1` FOREIGN KEY (`category_cat_id`) REFERENCES `category` (`cat_id`),
  CONSTRAINT `fk_material_model` FOREIGN KEY (`material_model`) REFERENCES `mat_model` (`mat_model_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `material`
--

LOCK TABLES `material` WRITE;
/*!40000 ALTER TABLE `material` DISABLE KEYS */;
INSERT INTO `material` VALUES (1,'Полиэтилен низкой плотности',1,1),(14,'Тестовый',1,28),(15,'123',1,29),(16,'dfghj',1,30),(17,'qwertyu',1,31);
/*!40000 ALTER TABLE `material` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `material_has_property`
--

DROP TABLE IF EXISTS `material_has_property`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `material_has_property` (
  `material_material_id` int NOT NULL,
  `property_prop_id` int NOT NULL,
  `value` double DEFAULT NULL,
  PRIMARY KEY (`material_material_id`,`property_prop_id`),
  KEY `fk_material_has_property_property1_idx` (`property_prop_id`),
  KEY `fk_material_has_property_material_idx` (`material_material_id`),
  CONSTRAINT `fk_material_has_property_material` FOREIGN KEY (`material_material_id`) REFERENCES `material` (`material_id`),
  CONSTRAINT `fk_material_has_property_property1` FOREIGN KEY (`property_prop_id`) REFERENCES `property` (`prop_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `material_has_property`
--

LOCK TABLES `material_has_property` WRITE;
/*!40000 ALTER TABLE `material_has_property` DISABLE KEYS */;
INSERT INTO `material_has_property` VALUES (1,1,920),(1,2,120),(1,3,2300),(14,1,45678),(14,2,159),(14,3,14),(15,3,7578),(16,1,123),(17,1,324567);
/*!40000 ALTER TABLE `material_has_property` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `property`
--

DROP TABLE IF EXISTS `property`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `property` (
  `prop_id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(25) NOT NULL,
  `category_cat_id` int NOT NULL DEFAULT '1',
  `unit` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`prop_id`),
  UNIQUE KEY `title_UNIQUE` (`title`),
  KEY `fk_property_category1_idx` (`category_cat_id`),
  KEY `fk_property_unit_idx` (`unit`),
  CONSTRAINT `fk_property_category1` FOREIGN KEY (`category_cat_id`) REFERENCES `category` (`cat_id`),
  CONSTRAINT `fk_property_unit` FOREIGN KEY (`unit`) REFERENCES `unit` (`unit_title`) ON DELETE SET NULL ON UPDATE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `property`
--

LOCK TABLES `property` WRITE;
/*!40000 ALTER TABLE `property` DISABLE KEYS */;
INSERT INTO `property` VALUES (1,'Плотность',1,'кг*м^3'),(2,'Температура плавления',1,'°С'),(3,'Удельная теплоёмкость',1,'Дж/(кг*°С)'),(5,'теплопроводность',1,'Вт/(м*С)');
/*!40000 ALTER TABLE `property` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `unit`
--

DROP TABLE IF EXISTS `unit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `unit` (
  `unit_title` varchar(15) NOT NULL,
  PRIMARY KEY (`unit_title`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `unit`
--

LOCK TABLES `unit` WRITE;
/*!40000 ALTER TABLE `unit` DISABLE KEYS */;
INSERT INTO `unit` VALUES ('°С'),('Вт/(м*С)'),('Вт/(м^2*°С)'),('Дж/(кг*°С)'),('Дж/моль'),('кг*м^3'),('м'),('м/с'),('Па*с^n');
/*!40000 ALTER TABLE `unit` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `login` varchar(10) NOT NULL,
  `password` varchar(15) NOT NULL,
  `category_cat_id` int NOT NULL,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `login_UNIQUE` (`login`),
  KEY `fk_user_category1_idx` (`category_cat_id`),
  CONSTRAINT `fk_user_category1` FOREIGN KEY (`category_cat_id`) REFERENCES `category` (`cat_id`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (1,'amogus','1234',1),(2,'imposter','4321',2),(18,'f','f',2);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-05-19  2:46:18
