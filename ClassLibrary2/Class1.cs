using System;
using Xunit;


// Интерфейс для стратегии вычисления площади
public interface IAreaCalculationStrategy
{
    double CalculateArea();
    bool IsRightTriangle();
}

// Класс для стратегии вычисления площади круга
public class CircleAreaCalculationStrategy : IAreaCalculationStrategy
{
    private readonly double _radius;

    public CircleAreaCalculationStrategy(double radius)
    {
        _radius = radius;
    }

    public double CalculateArea()
    {
        return Math.PI * _radius * _radius;
    }

    public bool IsRightTriangle()
    {
        return false; // Круг не является треугольником
    }
}

// Класс для стратегии вычисления площади треугольника
public class TriangleAreaCalculationStrategy : IAreaCalculationStrategy
{
    private readonly double _side1;
    private readonly double _side2;
    private readonly double _side3;

    public TriangleAreaCalculationStrategy(double side1, double side2, double side3)
    {
        _side1 = side1;
        _side2 = side2;
        _side3 = side3;
    }

    public double CalculateArea()
    {
        double p = (_side1 + _side2 + _side3) / 2;
        return Math.Sqrt(p * (p - _side1) * (p - _side2) * (p - _side3));
    }

    public bool IsRightTriangle()
    {
        double[] sides = new double[] { _side1, _side2, _side3 };
        Array.Sort(sides);
        return sides[0] * sides[0] + sides[1] * sides[1] == sides[2] * sides[2];
    }
}

// Фабрика для создания стратегии вычисления площади
public class AreaCalculationStrategyFactory
{
    public IAreaCalculationStrategy CreateStrategy(string shapeType, params double[] parameters)
    {
        switch (shapeType)
        {
            case "circle":
                if (parameters.Length == 1)
                {
                    double radius = parameters[0];
                    return new CircleAreaCalculationStrategy(radius);
                }
                break;
            case "triangle":
                if (parameters.Length == 3)
                {
                    double side1 = parameters[0];
                    double side2 = parameters[1];
                    double side3 = parameters[2];
                    return new TriangleAreaCalculationStrategy(side1, side2, side3);
                }
                break;
        }
        throw new ArgumentException("Invalid shape type or parameters");
    }

}
//Тесты unit
public class ShapeCalculatorTests
{
    [Fact]
    public void CalculateArea_Circle_ReturnsCorrectArea()
    {
        // Arrange
        var factory = new AreaCalculationStrategyFactory();
        var circleCalculator = factory.CreateStrategy("circle", 5);

        // Act
        double area = circleCalculator.CalculateArea();

        // Assert
        Assert.Equal(Math.PI * 5 * 5, area);
    }

    [Fact]
    public void CalculateArea_Triangle_ReturnsCorrectArea()
    {
        // Arrange
        var factory = new AreaCalculationStrategyFactory();
        var triangleCalculator = factory.CreateStrategy("triangle", 3, 4, 5);

        // Act
        double area = triangleCalculator.CalculateArea();

        // Assert
        Assert.Equal(6, area);
    }

    [Fact]
    public void CalculateArea_InvalidShapeType_ThrowsArgumentException()
    {
        // Arrange
        var factory = new AreaCalculationStrategyFactory();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => factory.CreateStrategy("invalid", 5));
    }

    [Fact]
    public void CalculateArea_InvalidParameters_ThrowsArgumentException()
    {
        // Arrange
        var factory = new AreaCalculationStrategyFactory();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => factory.CreateStrategy("triangle", 3, 4));
    }

    [Fact]
    public void IsRightTriangle_ValidTriangle_ReturnsTrue()
    {
        // Arrange
        var strategy = new TriangleAreaCalculationStrategy(3, 4, 5);

        // Act
        bool isRightTriangle = strategy.IsRightTriangle();

        // Assert
        Assert.True(isRightTriangle);
    }

    [Fact]
    public void IsRightTriangle_InvalidTriangle_ReturnsFalse()
    {
        // Arrange
        var strategy = new TriangleAreaCalculationStrategy(3, 4, 6);

        // Act
        bool isRightTriangle = strategy.IsRightTriangle();

        // Assert
        Assert.False(isRightTriangle);
    }

    [Fact]
    public void IsRightTriangle_Circle_ReturnsFalse()
    {
        // Arrange
        var strategy = new CircleAreaCalculationStrategy(5);

        // Act
        bool isRightTriangle = strategy.IsRightTriangle();

        // Assert
        Assert.False(isRightTriangle);
    }

    [Fact]
    public void CreateStrategy_Circle_WithSingleParameter_CreatesCircleStrategy()
    {
        // Arrange
        var factory = new AreaCalculationStrategyFactory();

        // Act
        var strategy = factory.CreateStrategy("circle", 5);

        // Assert
        Assert.IsType<CircleAreaCalculationStrategy>(strategy);
    }

    [Fact]
    public void CreateStrategy_Triangle_WithThreeParameters_CreatesTriangleStrategy()
    {
        // Arrange
        var factory = new AreaCalculationStrategyFactory();

        // Act
        var strategy = factory.CreateStrategy("triangle", 3, 4, 5);

        // Assert
        Assert.IsType<TriangleAreaCalculationStrategy>(strategy);
    }

    [Fact]
    public void CreateStrategy_InvalidShapeType_ThrowsArgumentException()
    {
        // Arrange
        var factory = new AreaCalculationStrategyFactory();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => factory.CreateStrategy("invalid", 5));
    }

    [Fact]
    public void CreateStrategy_Circle_WithMultipleParameters_ThrowsArgumentException()
    {
        // Arrange
        var factory = new AreaCalculationStrategyFactory();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => factory.CreateStrategy("circle", 5, 10));
    }

    [Fact]
    public void CreateStrategy_Triangle_WithSingleParameter_ThrowsArgumentException()
    {
        // Arrange
        var factory = new AreaCalculationStrategyFactory();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => factory.CreateStrategy("triangle", 3));
    }

    [Fact]
    public void CreateStrategy_Triangle_WithFourParameters_ThrowsArgumentException()
    {
        // Arrange
        var factory = new AreaCalculationStrategyFactory();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => factory.CreateStrategy("triangle", 3, 4, 5, 6));
    }

    // Дополнительные тесты для проверки треугольников

    [Fact]
    public void IsRightTriangle_TriangleWithNegativeSide_ReturnsFalse()
    {
        // Arrange
        var strategy = new TriangleAreaCalculationStrategy(-3, 4, 5);

        // Act
        bool isRightTriangle = strategy.IsRightTriangle();

        // Assert
        Assert.False(isRightTriangle);
    }

    [Fact]
    public void IsRightTriangle_TriangleWithZeroSide_ReturnsFalse()
    {
        // Arrange
        var strategy = new TriangleAreaCalculationStrategy(3, 0, 5);

        // Act
        bool isRightTriangle = strategy.IsRightTriangle();

        // Assert
        Assert.False(isRightTriangle);
    }

    [Fact]
    public void IsRightTriangle_TriangleWithAllNegativeSides_ReturnsFalse()
    {
        // Arrange
        var strategy = new TriangleAreaCalculationStrategy(-3, -4, -5);

        // Act
        bool isRightTriangle = strategy.IsRightTriangle();

        // Assert
        Assert.False(isRightTriangle);
    }

    [Fact]
    public void IsRightTriangle_TriangleWithAllZeroSides_ReturnsFalse()
    {
        // Arrange
        var strategy = new TriangleAreaCalculationStrategy(0, 0, 0);

        // Act
        bool isRightTriangle = strategy.IsRightTriangle();

        // Assert
        Assert.False(isRightTriangle);
    }

    [Fact]
    public void IsRightTriangle_TriangleWithOneNegativeSide_ReturnsFalse()
    {
        // Arrange
        var strategy = new TriangleAreaCalculationStrategy(3, 4, -5);

        // Act
        bool isRightTriangle = strategy.IsRightTriangle();

        // Assert
        Assert.False(isRightTriangle);
    }

    [Fact]
    public void IsRightTriangle_TriangleWithOneZeroSide_ReturnsFalse()
    {
        // Arrange
        var strategy = new TriangleAreaCalculationStrategy(0, 4, 5);

        // Act
        bool isRightTriangle = strategy.IsRightTriangle();

        // Assert
        Assert.False(isRightTriangle);
    }

    [Fact]
    public void IsRightTriangle_TriangleWithAllEqualSides_ReturnsTrue()
    {
        // Arrange
        var strategy = new TriangleAreaCalculationStrategy(5, 5, 5);

        // Act
        bool isRightTriangle = strategy.IsRightTriangle();

        // Assert
        Assert.True(isRightTriangle);
    }

    [Fact]
    public void IsRightTriangle_TriangleWithTwoEqualSides_ReturnsFalse()
    {
        // Arrange
        var strategy = new TriangleAreaCalculationStrategy(4, 4, 5);

        // Act
        bool isRightTriangle = strategy.IsRightTriangle();

        // Assert
        Assert.False(isRightTriangle);
    }

    [Fact]
    public void IsRightTriangle_TriangleWithNoEqualSides_ReturnsFalse()
    {
        // Arrange
        var strategy = new TriangleAreaCalculationStrategy(3, 4, 6);

        // Act
        bool isRightTriangle = strategy.IsRightTriangle();

        // Assert
        Assert.False(isRightTriangle);
    }

}


