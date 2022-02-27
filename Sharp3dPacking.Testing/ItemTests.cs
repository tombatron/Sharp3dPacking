using Xunit;

namespace Sharp3dPacking.Testing;

public class ItemTests
{
    public class IntersectsWithWill
    {
        [Theory]
        [MemberData(nameof(IntersectingItems))]
        public void DetectIntersections(Item item1, Item item2) =>
            Assert.True(item1.IntersectsWith(item2));

        public static IEnumerable<object[]> IntersectingItems
        {
            get
            {
                var item1 = new Item("50g [powder 3]", 3.9370m, 1.9685m, 1.9685m, 3)
                {
                    Position = new Position(7.874m, 0, 0),
                    RotationType = RotationType.DepthHeightWidth
                };

                var item2 = new Item("250g [powder 4]", 7.8740m, 3.9370m, 1.9685m, 4)
                {
                    Position = new Position(7.874m, 0, 0),
                    RotationType = RotationType.HeightWidthDepth
                };

                yield return new object[] {item1, item2};
                
                item1 = new Item("250g [powder 4]", 7.874m, 3.937m, 1.968m, 4)
                {
                    Position = new Position(11.811m, 0, 0),
                    RotationType = RotationType.WidthHeightDepth
                };

                item2 = new Item("250g [powder 7]", 7.8740m, 3.9370m, 1.9685m, 7)
                {
                    Position = new Position(7.874m, 1.968m, 0),
                    RotationType = RotationType.WidthHeightDepth
                };

                yield return new object[] {item1, item2};
            }
        }
    }
}