using FluentAssertions;
using NUnit.Framework;

namespace Reflection.Task
{
    [TestFixture]
    public class SafeObjectCreatorShould
    {
        private readonly ISafeObjectCreator objectCreator = new SafeObjectCreator();

        [Test]
        public void CreateSafe_ShouldWorkWithEmptyObject()
        {
            var obj = objectCreator.CreateSafe<object>();
            obj.Should().NotBeNull();
        }

        private class RecursiveTestCaseClass
        {
            public RecursiveTestCaseClass RecursiveProperty { get; set; }
        }

        [Test]
        public void CreateSafe_ThrowExceptionInRecursiveCase()
        {
            Assert.Throws<RecursiveOperationException>(() => objectCreator.CreateSafe<RecursiveTestCaseClass>());
        }

        private class InitializePublicMembersTestCaseClass
        {
            public object Property { get; set; }
            public object Field;
        }

        [Test]
        public void CreateSafe_InitializePublicMembers()
        {
            var obj = objectCreator.CreateSafe<InitializePublicMembersTestCaseClass>();

            obj.Property.Should().NotBeNull();
            obj.Field.Should().NotBeNull();
        }

        private class InitializePrivateMembersTestCaseClass
        {
            private object Property { get; set; }
            private object Field;

            public object GetProperty() => Property;
            public object GetField() => Field;
        }

        [Test]
        public void CreateSafe_InitializePrivateMembers()
        {
            var obj = objectCreator.CreateSafe<InitializePrivateMembersTestCaseClass>();

            obj.GetProperty().Should().NotBeNull();
            obj.GetField().Should().NotBeNull();
        }

        private class RootTestCaseClass
        {
            public ChildrenTestCaseClass PropertyLvl1 { get; set; }

            public ChildrenTestCaseClass FieldLvl1;
        }

        private class ChildrenTestCaseClass
        {
            public object PropertyLvl2 { get; set; }

            public object FieldLvl2;
        }

        [Test]
        public void CreateSafe_InitializePublicRootAndPublicChild()
        {
            var obj = objectCreator.CreateSafe<RootTestCaseClass>();

            obj.FieldLvl1.Should().NotBeNull();
            obj.PropertyLvl1.Should().NotBeNull();

            obj.FieldLvl1.FieldLvl2.Should().NotBeNull();
            obj.FieldLvl1.PropertyLvl2.Should().NotBeNull();

            obj.PropertyLvl1.FieldLvl2.Should().NotBeNull();
            obj.PropertyLvl1.PropertyLvl2.Should().NotBeNull();
        }
    }
}