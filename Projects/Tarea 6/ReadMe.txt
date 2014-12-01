+--------------------------+
|Tarea 6 - Editor meep-meep|
+--------------------------+

1 Introduccion
	1.1
2 Funciones
	2.1 Editor
		2.1.1 Inicio
			Al ingresar una ip válida, con un servidor corriendo, el editor se conecta al
			servidor. En caso de caída, el editor se cerrará, y preguntará si desea continuar offline
		2.1.2 Capas
			Al conectarse a un servidor, se le permitirá agregar y eliminar capas. Estas están marcadas con su fecha de
			creación
		2.1.3 Imágenes
			Para agregar una imagen, haga click en el botón correspondiente. Todos los botones tienen un tooltip asociado
			para facilitar su identificación
			Luego, haga click en cualquier parte de la capa en que quepa su imagen. Esta aparecerá de inmediato, segun el 
			tamaño especificado en la seccion inferior derecha del editor.
		2.1.4 Herramientas
			Todas las herramientas utilizan el selector de color y transparencia(Alpha) ubicado a la derecha-medio.
			El cursor permanecerá con la herramienta seleccionada, hasta que se escoja otra
			2.1.4.1 Cursor
				Borra la seleccion actual de herramienta, dejando solo el cursor común
			2.1.4.1 Pincel
				Crea un trazo que va apareciendo en tiempo real en los otros clientes. Utilza el grosor espcificado
				bajo la paleta de colores
			
		2.1.4 Guardado de imagenes
			Está ubicado en el menú Archivos, la unica opcion que trae. Permite guardar en 3 formatos diferentes.
			Esto guardará el contenido de la capa seleccionada actualmente.
	2.2 Servidor
		2.2.1 Panel
			El servidor presenta una interfaz que contiene un registro para observar los diversos eventos que ocurren.
			Es posible habilitar un modo de registro pesado, que registra eventos adicionales.
		
3 Notas y Errores
	3.1 Notas
		-Para propósitos de testeo, se permitió que el launcher creara más de un cliente
		-Existe un elevado consumo de CPU al crear mas de un cliente. 60% de uso sobre plataforma 
		 Intel core i3 3217U @ 1.8Ghz - 8 GB de ram. Con la version release esto disminuye un poco.
	3.2 Errores
		-No es posible arrastrar imágenes.
		-El decodificador JPEG es muy selectivo (mañoso), sugiero utilizar imagenes png o bmp. Arroja excepciones por los headers, indicando que estan dañados.
		-