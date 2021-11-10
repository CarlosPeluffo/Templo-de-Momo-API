-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 11-11-2021 a las 00:22:28
-- Versión del servidor: 10.4.20-MariaDB
-- Versión de PHP: 8.0.9

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `templo de momo`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `bibliotecas`
--

CREATE TABLE `bibliotecas` (
  `JuegoId` int(11) NOT NULL,
  `UsuarioId` int(11) NOT NULL,
  `fecha_seguido` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `comentarios`
--

CREATE TABLE `comentarios` (
  `id` int(11) NOT NULL,
  `NoticiaId` int(11) NOT NULL,
  `UsuarioId` int(11) NOT NULL,
  `fecha` datetime NOT NULL,
  `cuerpo` varchar(500) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `comentarios`
--

INSERT INTO `comentarios` (`id`, `NoticiaId`, `UsuarioId`, `fecha`, `cuerpo`) VALUES
(2, 1, 1, '2021-10-24 12:23:54', 'El mejor juego FPS :\'D'),
(3, 1, 2, '2021-10-24 15:54:02', 'Mueran, mostros mueran! >:| '),
(4, 3, 2, '2021-10-24 15:56:23', 'Que bueno que ahora se pueda poner en español El tank me hace acordar a mi viejo cuando se pone en pedo y nos quiere cagar a bifes a todos xd');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `creadores`
--

CREATE TABLE `creadores` (
  `id` int(11) NOT NULL,
  `nick_name` varchar(200) NOT NULL,
  `mail` varchar(200) NOT NULL,
  `password` varchar(200) NOT NULL,
  `nombre` varchar(200) NOT NULL,
  `apellido` varchar(200) NOT NULL,
  `avatar` varchar(200) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `creadores`
--

INSERT INTO `creadores` (`id`, `nick_name`, `mail`, `password`, `nombre`, `apellido`, `avatar`) VALUES
(3, 'AyrtonPR', 'ayrtonPR@mail.com', 'p3yd/EO1ONgK1V/zKqvkO2kkrK690UMftTYenBRomaU=', 'Ayrton', 'Peluffo', '/UsersFiles\\photo_3.png'),
(4, 'Killah', 'killah@mail.com', 'p3yd/EO1ONgK1V/zKqvkO2kkrK690UMftTYenBRomaU=', 'Agustina', 'Ortiz', '/UsersFiles\\photo_4.png');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `juegos`
--

CREATE TABLE `juegos` (
  `id` int(11) NOT NULL,
  `CreadorId` int(11) NOT NULL,
  `titulo` varchar(200) NOT NULL,
  `portada` varchar(200) DEFAULT NULL,
  `descripcion` varchar(500) NOT NULL,
  `requisitos` varchar(500) DEFAULT NULL,
  `precio` double DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `juegos`
--

INSERT INTO `juegos` (`id`, `CreadorId`, `titulo`, `portada`, `descripcion`, `requisitos`, `precio`) VALUES
(2, 3, 'Serious Sam The First Encounter', '/UsersFiles\\portada_2.jpg', 'Serious Sam es un FPS de acción arcade cooperativo de un solo juego y de alta adrenalina para 16 jugadores ', 'SO: Windows XP, Windows Vista o Windows 7 Procesador: AMD K6-3 a 400 MHz, Pentium II o Celeron-A a 300 MHz Memoria: 64 MB de RAM Disco Duro: 600 MB de espacio libre ', 78.99),
(3, 3, 'Serious Sam The Second Encounter', '/UsersFiles\\portada_3.jpg', 'La esperada secuela de acción y arcade en primera persona de Serious Sam: The First Encounter, es un juego de disparos arcade y de acción rebosante de adrenalina centrado en una frenética jugabilidad arcade de un jugador.', 'SO: Windows XP, Windows Vista o Windows 7 Procesador: AMD K6-3 a 400 MHz, Pentium II o Celeron-A a 300 MHz Memoria: 64 MB de RAM Disco Duro: 450 MB de espacio libre', 78.99),
(4, 4, 'Left 4 Dead', '/UsersFiles\\portada_4.jpg', 'Una nueva aventura que te meterá en la piel de uno de los cuatro \"supervivientes\" que libran una pugna de proporciones épicas contra hordas de zombis y sus terroríficas variedades mutantes especiales.', 'SO: Windows 7 32/64 bits / Vista 32/64 bits / XP Procesador: Pentium 4 a 3 GHz Memoria: 1 GB de RAM Disco Duro: 7.5 GB de espacio libre', 129.99),
(5, 4, 'Fran Bow', '/UsersFiles\\portada_5.jpg', 'Fran Bow es un espeluznante juego de aventuras que cuenta la historia de Fran, una joven que lucha contra un trastorno mental y un destino injusto.', 'SO: 7+\nProcesador: 1.7 GHz Dual Core\nMemoria: 2 GB de RAM\nGráficos: NVIDIA GeForce GTX 260, ATI Radeon 4870 HD, or equivalent card with at least 512 MB VRAM\nAlmacenamiento: 600 MB de espacio disponibles', 179.99);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `noticias`
--

CREATE TABLE `noticias` (
  `id` int(11) NOT NULL,
  `CreadorId` int(11) NOT NULL,
  `JuegoId` int(11) NOT NULL,
  `fecha` date NOT NULL,
  `titulo` varchar(200) NOT NULL,
  `cuerpo` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `noticias`
--

INSERT INTO `noticias` (`id`, `CreadorId`, `JuegoId`, `fecha`, `titulo`, `cuerpo`) VALUES
(1, 3, 2, '2021-10-23', 'Actualización', 'Actualizamos el modo multijugar para aumentar la fluidez y mejorar los fps online.'),
(2, 3, 2, '2021-10-24', 'Muy pronto la Secuela!', 'Muy pronto se lanzara oficialmente la segunda entrega y continuacion de este Famoso e Increible juego. Están avisado!s'),
(3, 4, 4, '2021-10-24', 'Parche De Idioma', 'Se agrego el parche de actualizacion de idiomas para que puedas disfrutarlos doblado o subtitulado al español (*u*)'),
(4, 3, 3, '2021-10-24', 'Actualización', 'Reparacion de bugs y contadores de daño :)'),
(5, 3, 2, '2021-10-24', 'Actualización de Hallowen', 'Nuevo diseño en los mapas multijugador con tematica de Hallowen. Buuuuh! :O'),
(6, 3, 3, '2021-11-03', 'Probando Editar una Noticia', 'Esto es una prueba para crear una noticia por primera vez :D');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `id` int(11) NOT NULL,
  `nick_name` varchar(200) NOT NULL,
  `mail` varchar(200) NOT NULL,
  `password` varchar(200) NOT NULL,
  `avatar` varchar(200) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`id`, `nick_name`, `mail`, `password`, `avatar`) VALUES
(1, 'Magnus Mefisto', 'marito@mail.com', 'p3yd/EO1ONgK1V/zKqvkO2kkrK690UMftTYenBRomaU=', '/UsersFiles\\photo_1.jpg'),
(2, 'Ricardo Iorio', 'ricardo@mail.com', 'p3yd/EO1ONgK1V/zKqvkO2kkrK690UMftTYenBRomaU=', '/UsersFiles\\photo_2.jpg');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `bibliotecas`
--
ALTER TABLE `bibliotecas`
  ADD PRIMARY KEY (`JuegoId`,`UsuarioId`),
  ADD KEY `UsuarioId` (`UsuarioId`),
  ADD KEY `JuegoId` (`JuegoId`);

--
-- Indices de la tabla `comentarios`
--
ALTER TABLE `comentarios`
  ADD PRIMARY KEY (`id`),
  ADD KEY `UsuarioId` (`UsuarioId`),
  ADD KEY `NoticiaId` (`NoticiaId`);

--
-- Indices de la tabla `creadores`
--
ALTER TABLE `creadores`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `mail` (`mail`);

--
-- Indices de la tabla `juegos`
--
ALTER TABLE `juegos`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `titulo` (`titulo`),
  ADD KEY `CreadorId` (`CreadorId`);

--
-- Indices de la tabla `noticias`
--
ALTER TABLE `noticias`
  ADD PRIMARY KEY (`id`),
  ADD KEY `CreadorId` (`CreadorId`),
  ADD KEY `JuegoId` (`JuegoId`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `mail` (`mail`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `comentarios`
--
ALTER TABLE `comentarios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `creadores`
--
ALTER TABLE `creadores`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `juegos`
--
ALTER TABLE `juegos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `noticias`
--
ALTER TABLE `noticias`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `bibliotecas`
--
ALTER TABLE `bibliotecas`
  ADD CONSTRAINT `bibliotecas_ibfk_1` FOREIGN KEY (`UsuarioId`) REFERENCES `usuarios` (`id`),
  ADD CONSTRAINT `bibliotecas_ibfk_2` FOREIGN KEY (`JuegoId`) REFERENCES `juegos` (`id`);

--
-- Filtros para la tabla `comentarios`
--
ALTER TABLE `comentarios`
  ADD CONSTRAINT `comentarios_ibfk_1` FOREIGN KEY (`UsuarioId`) REFERENCES `usuarios` (`id`),
  ADD CONSTRAINT `comentarios_ibfk_2` FOREIGN KEY (`NoticiaId`) REFERENCES `noticias` (`id`);

--
-- Filtros para la tabla `juegos`
--
ALTER TABLE `juegos`
  ADD CONSTRAINT `juegos_ibfk_1` FOREIGN KEY (`CreadorId`) REFERENCES `creadores` (`id`);

--
-- Filtros para la tabla `noticias`
--
ALTER TABLE `noticias`
  ADD CONSTRAINT `noticias_ibfk_1` FOREIGN KEY (`CreadorId`) REFERENCES `creadores` (`id`),
  ADD CONSTRAINT `noticias_ibfk_2` FOREIGN KEY (`JuegoId`) REFERENCES `juegos` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
