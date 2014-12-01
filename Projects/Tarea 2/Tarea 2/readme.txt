+-------------------------------------------+
|Readme Tarea 2 - Intérprete de tablas Fadic|
+-------------------------------------------+

0 Introduccion

	Apretando ctrl+c puede cerrar el programa en cualquier momento

1 Instrucciones
	1.1 Escogiendo un modo:
		
		En la primera pantalla se le preguntará que modo desea iniciar. Para escoger alguna opcion, debe mover 
		el cursor de seleccion con las teclas ARRIBA y ABAJO. 
		
		
	1.2 Modo juego:

		Al escoger el modo de juego, lo primero que se puede ver es el navegador del sistema de archivos. 
		Debe escoger un archivo de tipo XML válido. El programa impide escoger archivos con otras extensiones.
		Las carpetas estan marcadas con la etiqueta <dir>.
		Para mover el cursor, debe apretar los botones arriba y abajo para mover en las respectivas direcciones.
		Para abrir un mapa, debe apretar el botón espacio.
		La opcion ../ sube una carpeta en el sistema de archivos.
		
		1.2.1 Jugando
		
			Las mecánicas de juego son las mismas que se piden en el enunciado de la tarea 2
			Al crear la cantidad de conexiones requeridas, el juego finaliza. Es posiblee continuar por un nuevo juego o el editor
			
	1.3 Modo editor
		
		Primero debe escoger una carpeta en la que posicionará el archivo XML. Los mecanismos de movimiento 
		son iguales en todo el programa (Teclas arrbia-abajo). Es necesario escoger una carpeta en la que 
		se tengan permisos para leer y escribir, de lo contrario, habrá una excepcion. Para terminar la	
		navegacion y escoger la carpeta actual, debe escoger la opcion ./
		
		1.3.1 Inicializando el mapa
			
			Al escoger la carpeta se le pedirán los siguientes parametros básios
			
			Nombre : string : Un nombre arbitrario; no puede comenzar con tabs ni espacios
			
			Ancho : int : Un ancho inferior a 79; la consola no soporta mas ancho
			
			Largo: int :  Largo del mapa; arbitrario
			
		1.3.2 Editando el mapa
			
			Al principio se le presenta la pantalla de inicio
			
			Nombre = 12.xml | Largo = 12 - Ancho = 12
			Modo cursor : |█| Void
			[ ] Abrir paleta visual de colores
			████████████
			████████████
			████████████
			████████████
			████████████
			████████████
			████████████
			████████████
			████████████
			████████████
			████████████
			████████████
			
			1.3.2.1 Movimiento
			
				Para moverse en el mapa, utilize las teclas de movimiento para desplazarse por el mapa.
				Al apretar la tecla espacio, el programa crea un objeto, precisamente el que está en la line "Modo cursor"
				Si el modo cursor es un nodo de color, se le pedirá que cree otro nodo en otro lugar; esto es para evitar 
				ciertos errores que conllevan a un mapa sin sentido. El segundo nodo es posible ponerlo en un nodo ya puesto, para
				poder crear nodos impares.
				
			1.3.2.2 Encabezado
			
				El mapa, por defecto, está lleno de bloques vacios
				La primera linea presenta nombre del archivo, largo y ancho, los ultimos dos en cuadritos
				En la segunda linea, está el modo actual del cursor (Tambien es posible verlo como una especie de pincel)
				el cual representa lo que el cursor escribirá al presionar la tecla espacio.
				Finalmente, está la paleta visual de colores. Para seleccionarla, debe ir a la primera línea del mapa, y 
				subir una vez mas. Automáticamente el cursor se posicionara sobre el selector. Pulse espacio para abrir.
				
			1.3.2.3 Paleta visual de colores
			
				La paleta visual de colores permite un acceso facil a todos los colores; debe seleccionar uno con la tecla espacio
				
			1.3.2.4 Accesos directos
			
				Adicionalmente a la paleta visual, es posible cambiar el color de la paleta mediante el teclado
				Los accesos correspondientes son los siguentes
				
				Tecla
				W				Murallas
				V				Vacio
				P				Nodo Púrpura
				O				Nodo Naranja
				B				Nodo Azul
				R				Nodo Rojo
				G				Nodo Verde
				Y				Nodo Amarillo

			1.3.2.5 Finalizar
				Apretando la tecla esc, el editor guarda el mapa en un archivo XML para posterior uso, y se vuelve al menú principal

2 Notas
	- El color naranjo es representado con el color cyan, no existe naranjo en la consola.
	- Al crear el BMP necesario, se crea la imagen base en negro, pero el dibujo debido al tiempo que toma, no es funcional
	- Aún cuando el modo juego solo pueda abrir archivos XML, abrir un XML mal estructurado ocasionará una excepcion
	