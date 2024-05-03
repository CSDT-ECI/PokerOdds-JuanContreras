## Architectural Debt 📦

Teniendo en cuenta los enfoque vistos en clase, identificar algunos Architectural Smells, documentar en un archivo ArchitecturalSmells.md. (integrando con la versión unificada del proyecto), debe identificar y documentar adecuadamente algunas malas prácticas de arquitectura. Puede utilizar las referencias recomendadas o alguna otra que considere.

-------------------------------------
##  🛠️

_Descargue e instale el complemento Ndepend para generar  un diagrama de la solucion ya que actualmente esta carece de este y me facilitaria el trabajo de analisis. Al ejecutar el analisis se genra una nueva ruta en el proyecto PokerOdds-JuanContreras\NDependOut aqui encontraremos nuevos archivos usados en el anlisis entre estos NDependReport.html._
![Lint](/images/Arqui1.png)

_Otra informacion adicional que podemos ver gracias a  la prueba gratuita de esta aplicacion es lo siguiente_
![alt text](Arqui2.png)


_Utilizaremos ARCAN para continuar con el analisis, primero congifuramos la herramienta par que analice el codigo Original y ver en que estado se encontraba antes de todos los ajustes que le hemos realizado_
![alt text](Arqui4.png)
_El proyecto actual Cuenta con una alta Rigidez y un diseño de complegidad del 35 %_
![alt text](Arqui5.png)
_El Nivel de rigidez se detalla calaramente al visualizar el diagrama de dependencias, en este se ve un alto nicel de dependencias_
![alt text](Arqui6.png)
_El proyecto reporta 6 Architectural smells que son los que se muestran en la imagen, ahora vamos a  configurar nuestro poryecto, para verificar que nuestros ajustes hayan tenido algun cambio en la aplicacion._
![alt text](Arqui7.png)
_A nivel de Architectural smells el nivel de rigidez y de complegidad se mantienen, pero a nivel de deuda tecnica !hemos bajado los indices un poquitico profesor¡_
![alt text](Arqui8.png)
_El diagrama de dependencias ha tenido cambios con los ajsutes que hemo realizado pero aun mantiene un alto nivel de dependencias, esto quiere decir que a pesar de los ajustes el codigo inicial no fue pensado con muy buenas practicas ni pensando a futuro en terminos de mantenibilidad_
![alt text](Arqui9.png)
_El proyecto reporta 2 Architectural smells que son los que se muestran en la imagen, gracias a nuestros ajustes se solucionaron 4 de los reportados en el poryecto original, a continuacion mas detalle del otro Architectural smell, que ess una clase Dios, se solucionaria dividiendo sus responsabilidades en otras clases ya que contiene muchas lienas de codigo_
![alt text](Arqui10.png)

----------------------------------------------------------
## Designite(C#) 
_Se descarga y se ejecuta un analisis con el codigo de la siguiente forma._
![alt text](Arqui3.png)
_Nos sale el siguiente mensaje de error , al revisar los logs tambien podemos encontrar lo  siguiente_
![alt text](Arqui11.png)
![alt text](Arqui12.png)
![alt text](Arqui13.png)
_Al buscar en internet no encontre mucha informacion y al probar lo que parecai de utilidad el proyecto no se analizo, parecen ser temas de compatibilidad de la herramienta con la version .Net Framework 4.8_