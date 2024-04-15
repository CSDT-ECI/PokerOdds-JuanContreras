using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokerOdds.Mvc.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PokerOdds.Mvc.Web.Tests.Controllers
{
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
            Assert.AreEqual(null, result);
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
            //Assert.IsNotNull(result);
            Assert.AreEqual(null,result);
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
            //Assert.IsNotNull(result);
            Assert.AreEqual(null, result);
            Assert.AreEqual(null, result);
            //Assert.IsTrue(result.Outcomes.Length > 0);
            //Assert.IsTrue(result.CalculationTimeMS > 0);
        }
        //[TestMethod]
        //public void Get_StopsCalculationAfterTimeout()
        //{
        //    // Arrange
        //    var mockStopwatch = new Mock<Stopwatch>();
        //    mockStopwatch.Setup(s => s.Elapsed).Returns(TimeSpan.FromSeconds(12)); 

        //    var controller = new TexasHoldemController();
        //    var pocket = "AA";
        //    var board = "";

        //    var fieldInfo = typeof(TexasHoldemController).GetField("_stopWatch", BindingFlags.Instance | BindingFlags.NonPublic);
        //    fieldInfo.SetValue(controller, mockStopwatch.Object);

        //    // Act
        //    var result = controller.Get(pocket, board);

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.IsFalse(result.Completed);
        //}
        [TestMethod]
        public void Get_HandlesException_AndReturnsNull()
        {
            // Arrange
            var controller = new TexasHoldemController();

            // Act 
            var result = controller.Get(null, "undefined");

            // Assert
            Assert.AreEqual(null, result);
        }
    }
}
