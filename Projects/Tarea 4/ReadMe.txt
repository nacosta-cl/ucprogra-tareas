+--------------------+
|Tarea 4 - 2048 y más|
+--------------------+

=====Introduccion=====

Esta

Estructura general
	1.1 Backend
		Es un proyecto independiente, totalmente autónomo. Esta basado en una estructura de nodos 2D especiales.
		El algoritmo de movimiento toma cuadro por cuadro(A), y lo analiza con el cuadro de la direccion contraria al movimiento(B), teniendo asi varios 
		casos en los que se efectua movimiento.
			-A vale 0
				-B tiene algun valor = mov
			-A tiene valor
				-B tiene el mismo valor
					-A y B son sumables= mov
		El algoritmo se ejecuta n veces, simulando así una especie de "gravedad"
		Todo esto queda guardado en los nodos.		
		Es capaz de interpretar un arreglo de enteros, y asimilarlo en su estructura, o crear una representacion de si mismo en un arreglo de enteros.
	1.2 Frontend
		Es dependiente del Backend hasta cierto punto.
		Tiene un sistema tal que puede mantener multiples instancias generadas a partir de la ventana principal.
		
2 Detalles
	
	2.1 Slider
		Se agregó un slider en vez de una caja de texto, para evitar valores incorrectos, dar un rango para el mapa, y para mejorar la interaccion con el usuario.
		Tiene una etiqueta de actualizacion en tiempo real, para visualizar los valores
	2.2 Undo
		Undo funciona en base a serializacion, por lo tanto el programa debe tener permisos de escritura en la carpeta
	2.3 Puntuaciones altas (Highscores)
		Se incorporó una forma de guardar puntuaciones altas. Debido a las diferencias de dificultad asociadas al tamaño (Mas grande, mas facil ganar)
		se guardan las puntuaciones asociadas a un tamaño fijo n. Para cada tipo de arreglo, existe una puntuacion separada. Adicionalmente, la serializacion
		se hace en un archivo XML.
	2.4 Carga de tableros
		Se pueden cargar y guardar tableros mediante la interfaz especializada del sistema. Los tableros se manejan con la extensión .2ks
	2.5 Modo numerico
		Para mejorar la jugabilidad, existe un modo en el cual los cuadros se representan mediante numeros en vez de ayudantes. Los 				tableros guardados son intercompatibles con los de ayudantes

3 Bugs y cosas que faltan

	3.1 Faltas
		- Las animaciones no funcionan. Al final de "juego.xaml" están los intentos de código para animar, los que funcionaban de formas extrañas
		- Quizá sea necesario manejar el caso de la ausencia de la dll necesaria para trabajar.
	3.2 Bugs
		- Al iniciar un tablero, lso 2 cuadros iniciales no siempre son de valor 2