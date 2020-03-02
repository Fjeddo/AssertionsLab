using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests
{
    public class SingletonInScopedTests
    {
        [Fact]
        public void TheSingleton_should_be_able_to_use_scoped_stuff()
        {
            var serviceCollection = new ServiceCollection();

            // Service registrations
            serviceCollection.AddSingleton<IBus>(provider => new Bus(provider.GetService<IConfiguration<IBus>>()));
            serviceCollection.AddSingleton<IConfiguration<IBus>, BusConfiguration>();
            serviceCollection.AddSingleton<IConfiguration<ISomethingElse>, SomethingElseConfiguration>();
            
            serviceCollection.AddScoped<ISender, Sender>();

            // Build provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // The first way to create scoped instance
            var firstSender = serviceProvider.CreateScope().ServiceProvider.GetService<ISender>();
            var secondSender = serviceProvider.CreateScope().ServiceProvider.GetService<ISender>();

            // Assertions
            firstSender.Should().NotBeSameAs(secondSender);
            firstSender.Bus.Should().BeSameAs(secondSender.Bus);

            // The second way to create scoped instance
            ISender thirdSender;
            ISender fourthSender;
            using (var scope = serviceProvider.CreateScope())
            {
                thirdSender = scope.ServiceProvider.GetService<ISender>();
            }

            using (var scope = serviceProvider.CreateScope())
            {
                fourthSender = scope.ServiceProvider.GetService<ISender>();
            }

            // Assertions
            thirdSender.Should().NotBeSameAs(fourthSender);
            thirdSender.Should().NotBeSameAs(secondSender);
            thirdSender.Should().NotBeSameAs(firstSender);

            thirdSender.Bus.Should().BeSameAs(fourthSender.Bus);
            thirdSender.Bus.Should().BeSameAs(secondSender.Bus);
            thirdSender.Bus.Should().BeSameAs(firstSender.Bus);

            // Yet another way using no scopes, maybe the most straight forward?
            var anotherSender = serviceProvider.GetService<ISender>();

            // Assertions
            anotherSender.Should().NotBeSameAs(firstSender);
            anotherSender.Should().NotBeSameAs(secondSender);
            anotherSender.Should().NotBeSameAs(thirdSender);
            anotherSender.Should().NotBeSameAs(fourthSender);

            anotherSender.Bus.Should().BeSameAs(firstSender.Bus);
            anotherSender.Bus.Should().BeSameAs(secondSender.Bus);
            anotherSender.Bus.Should().BeSameAs(thirdSender.Bus);
            anotherSender.Bus.Should().BeSameAs(fourthSender.Bus);
        }
    }

    // This one depends on the bus
    public class Sender : ISender
    {
        public IBus Bus { get; }

        public Sender(IBus bus) => Bus = bus;
        public void SendSomething() => Bus.Publish(null);
    }

    public interface ISender
    {
        IBus Bus { get; }
        void SendSomething();
    }

    // The bus
    public class Bus : IBus
    {
        private readonly BusConfiguration _configuration;

        public Bus(IConfiguration<IBus> configuration) => _configuration = (BusConfiguration) configuration;
        public void Publish(object message) { }
    }

    public interface IBus
    {
        void Publish(object message);
    }

    public interface ISomethingElse { }

    // Configurations
    public interface IConfiguration<T> { }

    public class BusConfiguration : IConfiguration<IBus> { }
    public class SomethingElseConfiguration : IConfiguration<ISomethingElse> { }

}