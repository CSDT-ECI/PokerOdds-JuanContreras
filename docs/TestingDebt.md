#  Testing Debt #
- Identificar en su proyecto cuales prácticas de Testing Debt se presentan y documentar con ejemplos si aplica
- Si no existen pruebas unitarias en su proyecto proponer algunas pruebas y si ya existen proponer algunos escenarios complementarios para garantizar un mayor cubrimiento (Coverage) (Documentar ejemplos)
- Proponer algunas mejoras o ideas para reducir la deuda

## 1  📋
### Patrón AAA
Las pruebas unitarias que se encuentran actualmente en el proyecto, siguen el patrón AAA.
~~~csharp
 [TestMethod]
        public void GetById()
        {
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            string result = controller.Get(5);

            // Assert
            Assert.AreEqual("value", result);
        }
~~~
### Son pruebas unitarias no de integración
Se prueban un conjunto limitado de entradas y salidas dentro de un solo móduloprueban un conjunto limitado de entradas y salidas dentro de un solo módulo
~~~csharp
 [TestMethod]
        public void Get()
        {
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            IEnumerable<string> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }
~~~

### No permitan que las pruebas tengan dependencias con servicios experto (Isolated) utilice Mocks
Las pruebas unitarias no tienen dependencias con otros servicios.
~~~csharp

//Refactor :
//he encapsulado la lógica  en tres métodos (IterateOpponentHands, IterateBoards, y EvaluateHands) dividiendo el codigo en metodos mas pequeños
 public TexasHoldemOdds Get(string pocket, string board)
 {
    ...
 }

 private void IterateOpponentHands(ulong partialBoard, ulong playerMask, double[] playerWins, double[] opponentWins, ref long count, ref Stopwatch stopWatch)
 {
    ...
 }

 private void IterateBoards(ulong partialBoard, ulong playerMask, double[] playerWins, double[] opponentWins, ref long count, ref Stopwatch stopWatch, ulong opponentMask)
 {
    ...
 }

 private void EvaluateHands(ulong playerMask, ulong opponentMask, double[] playerWins, double[] opponentWins, ref long count, ref Stopwatch stopWatch, ulong boardMask)
 {
    ...
 }

 private void CreateOutcomes(double[] playerWins, long count, List<PokerOutcome> outcomes)
 {
   ...
 }
//ya corregido en codigo
~~~
### Divide y vencerás
Las pruebas unitarias se dirigen a un solo escenario a la vez
~~~csharp
 [TestMethod]
        public void Delete()
        {
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            controller.Delete(5);

            // Assert
        }
~~~
### Estándares de nombramiento
Los nombres de las pruebas unitarias son descriptivos e indican claramente qué se está probando.

1. public void Delete()
2. public void Put()
3. public void Post()
4. public void GetById()
5. public void Get()

## 2 ⚙️
###  Coverage
Se agregan las siguientes pruebas para aumentar el coverage del proyecto.
~~~csharp
 [TestClass]
 public class TexasHoldemControllerTest
 {


     [TestMethod]
     public void Get_ThrowsArgumentNullException_ForEmptyPocket()
     {
         // Arrange
         var controller = new TexasHoldemController();

         // Act 
         var result = controller.Get(null, "undefined");

         //Assert
         Assert.Equals(result, null);
     }

     [TestMethod]
     public void Get_HandlesUndefinedBoardAsEmptyBoard()
     {
         // Arrange
         var controller = new TexasHoldemController();
         var pocket = "pocket";

         // Act
         var result = controller.Get(pocket, "undefined");

         // Assert
         Assert.IsNotNull(result);
         Assert.Equals(string.Empty, result.Board);
     }
     [TestMethod]
     public void Get_ReturnsTexasHoldemOdds_ForValidInput()
     {
         // Arrange
         var controller = new TexasHoldemController();
         var pocket = "AA";
         var board = "KKK";

         // Act
         var result = controller.Get(pocket, board);

         // Assert
         Assert.IsNotNull(result);
         Assert.Equals(pocket, result.Pocket);
         Assert.Equals(board, result.Board);
         Assert.IsTrue(result.Outcomes.Length > 0);
         Assert.IsTrue(result.CalculationTimeMS > 0);
     }
     [TestMethod]
     public void Get_StopsCalculationAfterTimeout()
     {
         // Arrange
         var mockStopwatch = new Mock<Stopwatch>();
         mockStopwatch.Setup(s => s.Elapsed).Returns(TimeSpan.FromSeconds(12)); 

         var controller = new TexasHoldemController();
         var pocket = "AA";
         var board = "";

         var fieldInfo = typeof(TexasHoldemController).GetField("_stopWatch", BindingFlags.Instance | BindingFlags.NonPublic);
         fieldInfo.SetValue(controller, mockStopwatch.Object);

         // Act
         var result = controller.Get(pocket, board);

         // Assert
         Assert.IsNotNull(result);
         Assert.IsFalse(result.Completed);
     }
     [TestMethod]
     public void Get_HandlesException_AndReturnsNull()
     {
         // Arrange
         var controller = new TexasHoldemController();

         // Act 
         var result = controller.Get(null, "undefined");

         // Assert
         Assert.Equals(result, null);
     }
 }
~~~
## 3  📌
###  Propuestas para reducir la deuda
1. Implementar pruebas unitarias:

- Escribir pruebas unitarias para las funcionalidades clave del proyecto.
- Asegurar una buena cobertura de código con las pruebas.

2. Refactorizar el código:
- Eliminar código duplicado y mejorar la legibilidad del código.
- Implementar patrones de diseño para mejorar la estructura del código.
- 
3. Mejorar la documentación:

- Documentar las funcionalidades del proyecto, incluyendo código, APIs y arquitectura.
- Mantener la documentación actualizada con los cambios en el código.
- Utilizar herramientas de generación de documentación automática.

4. Modernizar el proyecto:
  (la version de .net con la que venia el proyecto era muy vieja se actualizo pero aun se puede   actualizar mas a tecnologias aun con mantenimiento)
- Actualizar las librerías y frameworks a las últimas versiones.
- Adoptar nuevas tecnologías que puedan mejorar la eficiencia del proyecto.
- Evaluar la posibilidad de migrar el proyecto a una arquitectura más moderna.

5. Monitoreo y medición:
- Implementar métricas para medir la calidad del código y la eficiencia del proceso de desarrollo.
- Monitorizar el proyecto para identificar y corregir posibles problemas.
- Utilizar las métricas para tomar decisiones informadas sobre la evolución del proyecto.

9. Gestión de la deuda técnica:

- Identificar y priorizar la deuda técnica del proyecto.
- Implementar un plan para reducir la deuda técnica de forma gradual.
- Asegurar que la reducción de la deuda técnica sea parte del proceso de desarrollo regular.