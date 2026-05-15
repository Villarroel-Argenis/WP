namespace WP.SourceGenerators.Tests;

/// <summary>
/// Pruebas unitarias para el generador de registros de CQRS, verificando que se generen correctamente los registros para CommandHandlers, QueryHandlers y Validators, y que se manejen casos sin implementaciones.
/// </summary>
public sealed class CqrsRegistrationGeneratorTests
{
    /// <summary>
    /// Prueba que verifica que el generador de código registre correctamente un Command Handler en el contenedor de dependencias. Se define una interfaz ICommandHandler, un comando de prueba y un handler de prueba, luego se ejecuta el generador y se verifica que el código generado contenga las referencias correctas al handler, la interfaz y el método de registro en el contenedor de dependencias.
    /// </summary>
    [Fact]
    public void GeneradorConCommandHandlerGeneraRegistro()
    {
        string source = """
                        public interface ICommandHandler<TCommand, TResult> { }
                        public sealed record TestCommand();
                        public sealed class TestCommandHandler : ICommandHandler<TestCommand, System.Guid> { }
                        """;

        string generatedCode = RunGenerator(source);

        generatedCode.ShouldContain("ICommandHandler<TestCommand, System.Guid>");
        generatedCode.ShouldContain("TestCommandHandler");
        generatedCode.ShouldContain("ServiceDescriptor.Scoped");
        generatedCode.ShouldContain("AddGeneratedServices");
    }

    /// <summary>
    /// Prueba que verifica que el generador de código registre correctamente un Query Handler en el contenedor de dependencias. Se define una interfaz IQueryHandler, una consulta de prueba y un handler de prueba, luego se ejecuta el generador y se verifica que el código generado contenga las referencias correctas al handler, la interfaz y el método de registro en el contenedor de dependencias.
    /// </summary>
    [Fact]
    public void GeneradorConQueryHandlerGeneraRegistro()
    {
        string source = """
                        public interface IQueryHandler<TQuery, TResult> { }
                        public sealed record TestQuery(System.Guid Id);
                        public sealed class TestQueryHandler : IQueryHandler<TestQuery, string> { }
                        """;

        string generatedCode = RunGenerator(source);

        generatedCode.ShouldContain("IQueryHandler<TestQuery, string>");
        generatedCode.ShouldContain("TestQueryHandler");
        generatedCode.ShouldContain("ServiceDescriptor.Scoped");
        generatedCode.ShouldContain("AddGeneratedServices");
    }

    /// <summary>
    /// Prueba que verifica que el generador de código registre correctamente un Validator en el contenedor de dependencias. Se define una interfaz IValidator, un comando de prueba y un validator de prueba, luego se ejecuta el generador y se verifica que el código generado contenga las referencias correctas al validator, la interfaz y el método de registro en el contenedor de dependencias.
    /// </summary>
    [Fact]
    public void GeneradorConValidatorGeneraRegistro()
    {
        string source = """
                        public interface IValidator<T> { }
                        public sealed record TestCommand(string Name);
                        public sealed class TestCommandValidator : IValidator<TestCommand> { }
                        """;

        string generatedCode = RunGenerator(source);

        generatedCode.ShouldContain("IValidator<TestCommand>");
        generatedCode.ShouldContain("TestCommandValidator");
        generatedCode.ShouldContain("ServiceDescriptor.Scoped");
        generatedCode.ShouldContain("AddGeneratedServices");
    }

    /// <summary>
    /// Prueba que verifica que el generador de código maneje correctamente el caso en el que no hay implementaciones de Command Handlers, Query Handlers o Validators. Se define una clase simple sin implementar ninguna de las interfaces relevantes, luego se ejecuta el generador y se verifica que el código generado contenga el método de registro pero no contenga referencias a handlers, interfaces o registros en el contenedor de dependencias.
    /// </summary>
    [Fact]
    public void GeneradorSinImplementacionesGeneraMetodoVacio()
    {
        string source = """
                        public class ClaseSimple { }
                        """;

        string generatedCode = RunGenerator(source);

        generatedCode.ShouldContain("AddGeneratedServices");
        generatedCode.ShouldNotContain("ICommandHandler");
        generatedCode.ShouldNotContain("IQueryHandler");
        generatedCode.ShouldNotContain("IValidator");
    }

    private static string RunGenerator(string source)
    {
        var compilation = CSharpCompilation.Create(
            assemblyName: "TestAssembly",
            syntaxTrees: [CSharpSyntaxTree.ParseText(source)],
            references: [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var generator = new CqrsRegistrationGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);
        driver = (CSharpGeneratorDriver)driver.RunGenerators(compilation);

        GeneratorDriverRunResult result = driver.GetRunResult();
        return result.GeneratedTrees
            .Select(t => t.GetText().ToString())
            .FirstOrDefault() ?? string.Empty;
    }
}
