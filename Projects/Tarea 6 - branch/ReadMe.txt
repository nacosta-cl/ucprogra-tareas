+--------------------------+
|Tarea 6 - Editor meep-meep|
+--------------------------+

1 Introduccion
	1.1
2 Funciones
	2.1 Editor
		2.1.1 Inicio
			Al ingresar una ip v�lida, con un servidor corriendo, el editor se conecta al
			servidor. En caso de ca�da, el editor se cerrar�, y preguntar� si desea continuar offline
		2.1.2 Capas
			Al conectarse a un servidor, se le permitir� agregar y eliminar capas. Estas est�n marcadas con su fecha de
			creaci�n
		2.1.3 Im�genes
			Para agregar una imagen, haga click en el bot�n correspondiente. Todos los botones tienen un tooltip asociado
			para facilitar su identificaci�n
			Luego, haga click en cualquier parte de la capa en que quepa su imagen. Esta aparecer� de inmediato, segun el 
			tama�o especificado en la seccion inferior derecha del editor.
		2.1.4 Herramientas
			Todas las herramientas utilizan el selector de color y transparencia(Alpha) ubicado a la derecha-medio.
			El cursor permanecer� con la herramienta seleccionada, hasta que se escoja otra
			2.1.4.1 Cursor
				Borra la seleccion actual de herramienta, dejando solo el cursor com�n
			2.1.4.1 Pincel
				Crea un trazo que va apareciendo en tiempo real en los otros clientes. Utilza el grosor espcificado
				bajo la paleta de colores
			
		2.1.4 Guardado de imagenes
			Est� ubicado en el men� Archivos, la unica opcion que trae. Permite guardar en 3 formatos diferentes.
			Esto guardar� el contenido de la capa seleccionada actualmente.
	2.2 Servidor
		2.2.1 Panel
			El servidor presenta una interfaz que contiene un registro para observar los diversos eventos que ocurren.
			Es posible habilitar un modo de registro pesado, que registra eventos adicionales.
		
3 Notas y Errores
	3.1 Notas
		-Para prop�sitos de testeo, se permiti� que el launcher creara m�s de un cliente
		-Existe un elevado consumo de CPU al crear mas de un cliente. 60% de uso sobre plataforma 
		 Intel core i3 3217U @ 1.8Ghz - 8 GB de ram. Con la version release esto disminuye un poco.
	3.2 Errores
		-No es posible arrastrar im�genes.
		-El decodificador JPEG es muy selectivo (ma�oso), sugiero utilizar imagenes png o bmp. Arroja excepciones por los headers, indicando que estan da�ados.
		-