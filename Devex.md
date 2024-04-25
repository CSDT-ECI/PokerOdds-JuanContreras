#  DevEx + Developer Productivity #
- Analizar el proyecto teniendo en cuenta las diferentes actividades que se han realizado e identificar puntos positivos o negativos con relación a la experiencia del desarrollador DevEx y la productividad (SPACE), documentar esto en la bitácora. Identificar oportunidades de mejora, algunas métricas identificables y lo que consideren importante con relación a los dos frameworks vistos en clase (DevEX + SPACE)
- Aplicar alguna herramienta AI para poder solucionar temas de malas prácticas en el desarrollo, documentación de código, entendimiento de código, implementación de pruebas unitarias, refactorización o lo que ustedes consideren. Hay muchas herramientas en el mercado, como mínimo utilizar GitHub Copilot o AWS CodeWhisperer  traten de explotar algunas de las características de este y documenten su beneficio y como mejora productividad y experiencia cuando se utilizan.

## Oportunidades de mejora: 📋
- Documentación: El repositorio carece de documentación clara y completa. Se recomienda agregar una descripción del proyecto, su objetivo, la arquitectura general, las dependencias utilizadas y las instrucciones para ejecutar la aplicación.
- Pruebas: No se observa una buenacantidad de  pruebas unitarias o de integración en el código. Implementar pruebas automatizadas ayuda a garantizar la calidad del código y facilita la detección de errores, a pesar de que en anteriores entregas se realizaron mas pruebas, aun el coverage se encuentra en un nivel que de ser un entorno real no pasaria atributos de calidad.
- Manejo de errores: El código no maneja errores de forma explícita. Se recomienda implementar mecanismos para identificar y manejar errores de forma robusta.
- Modularidad: El código podría dividirse en módulos más pequeños y cohesivos para mejorar la legibilidad, mantenibilidad y reutilización del código, Esto ya se ha venido haciendo en anteriores entregas , asi como se modificaronalgunas clases aun falta revisar a detalle el resto.
- Formato del código: Se recomienda aplicar un formato de código consistente para mejorar la legibilidad y facilitar el trabajo colaborativo.
- DevEX: El proyecto podría beneficiarse de la implementación de prácticas como la integración continua, la entrega continua y la automatización de pruebas para mejorar la eficiencia del desarrollo y la calidad del código.
- SPACE: El código podría beneficiarse de la aplicación de principios de arquitectura como SOLID para mejorar la escalabilidad, mantenibilidad y flexibilidad del proyecto, a pesar de ya contar con algunas de estos principios, y en anteriores entregas se modfico codigo para adaptaarlo mejor , aun tiene mucho que se podria mejorar.
- Documentacion: Sibien varias clases cuentan con documentacion, esta no se mantiene en todo el codigo  lo que puede llegar a dificultar la legibilidad y mantenibilidaddel codigo.
## Métricas identificables:
- Cobertura de pruebas: El porcentaje de código cubierto por pruebas automatizadas.
- Tiempo de ejecución: El tiempo que tarda la aplicación en ejecutar diferentes tareas.
- Uso de memoria: La cantidad de memoria que utiliza la aplicación durante su ejecución.
- Número de errores: El número de errores encontrados durante la ejecución de la aplicación.
- Comentarios del usuario: La retroalimentación de los usuarios sobre la usabilidad y funcionalidad de la aplicación.

## IA
### GitHub Copilot
Para demostrar el potencial de esta herramienta y como nos apoya en labores que normalmente no se prorizan pero que perjudican la calidad del codigo , tomare como referencia la clase HoldemOddsCalculator y la mejoraremos con GitHub Copilot.

Para empezar el metodo Calculate es extenso , y complejo vamos apedirle a la IA ayuda para que sea mas facil de entender.
![Descripción de la imagen](/images/devex1.png)
Seguimos todos los consejos de Copilot , pero no todo es alegria y felicidad, la herramienta aun noes perfecta, genera algunos errores que tenemos que solucionar, por ende es importante verificar las respuestas que esta nos da.
![Descripción de la imagen](/images/devex2.png) ![Descripción de la imagen](/images/devex3.png)

Otra forma de utilizarlo que nos puede ahorrar bastante tiempo es generando documentación.
 ![Descripción de la imagen](/images/devex3.png)
 Con esto en muy poco tiempo reducimos la deuda tecnica, sin mucho esfuerzo y mejoramos la calidad del codigo.