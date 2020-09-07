using NeoServer.Game.Contracts.Items;
using NeoServer.Game.Contracts.Items.Types;
using NeoServer.Game.Contracts.World;
using NeoServer.Game.Enums.Location;
using NeoServer.Game.Enums.Location.Structs;
using NeoServer.Game.Items.Tests;
using NeoServer.Game.World.Map.Tiles;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NeoServer.Game.World.Tests
{

    public class RemoveThingTileTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {

            yield return new object[] { ItemTestData.CreateCumulativeItem(500, 100), 40, 500, 60 };
            yield return new object[] { ItemTestData.CreateCumulativeItem(500, 50), 49, 500, 1 };
            yield return new object[] { ItemTestData.CreateCumulativeItem(500, 50), 1, 500, 49 };
            yield return new object[] { ItemTestData.CreateCumulativeItem(500, 1), 1, 400, 32 };
            yield return new object[] { ItemTestData.CreateCumulativeItem(500, 100), 100, 400, 32 };
            yield return new object[] { ItemTestData.CreateCumulativeItem(500, 45), 45, 400, 32 };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
    public class TileTest
    {

        public static IEnumerable<object[]> NextTilesTestData =>
      new List<object[]>
      {
         new object[] {new Tile(new Coordinate(101, 100, 7), TileFlag.None, null, new IItem[0], new IItem[0]) },
         new object[] {new Tile(new Coordinate(101, 101, 7), TileFlag.None, null, new IItem[0], new IItem[0]) },
         new object[] {new Tile(new Coordinate(100, 101, 7), TileFlag.None, null, new IItem[0], new IItem[0]) },
         new object[] {new Tile(new Coordinate(99, 100, 7), TileFlag.None, null, new IItem[0], new IItem[0]) },
         new object[] {new Tile(new Coordinate(100, 99, 7), TileFlag.None, null, new IItem[0], new IItem[0]) },
         new object[] {new Tile(new Coordinate(99, 99, 7), TileFlag.None, null, new IItem[0], new IItem[0]) },
         new object[] {new Tile(new Coordinate(101, 99, 7), TileFlag.None, null, new IItem[0], new IItem[0]) },
         new object[] {new Tile(new Coordinate(99, 101, 7), TileFlag.None, null, new IItem[0], new IItem[0]) },
      };

        private Tile CreateTile(params IItem[] item)
        {
            var topItems = new List<IItem> {
                ItemTestData.CreateTopItem(id: 1, topOrder: 1)
            };
            var items = new List<IItem> {
                ItemTestData.CreateRegularItem(100),
                ItemTestData.CreateRegularItem(200)
            };
            items.AddRange(item);

            var tile = new Tile(new Coordinate(100, 100, 7), TileFlag.None, null, topItems.ToArray(), items.ToArray());
            return tile;
        }

        [Fact]
        public void Constructor_Given_Items_Creates_Tile()
        {
            var tile = new Tile(new Coordinate(100, 100, 7), TileFlag.None, null, new List<IItem>()
            {
                ItemTestData.CreateTopItem(id: 2, topOrder: 1)
            }.ToArray(), new List<IItem>()
            {
                ItemTestData.CreateRegularItem(id: 5),
                ItemTestData.CreateRegularItem(id: 6),
                ItemTestData.CreateRegularItem(id: 6),
                ItemTestData.CreateCumulativeItem(id: 7, amount: 35),
                ItemTestData.CreateCumulativeItem(id: 7, amount: 80),
                ItemTestData.CreateCumulativeItem(id:8, amount: 3)
            }.ToArray());

            var downItemsExpected = new List<IItem>
            {
                ItemTestData.CreateRegularItem(id: 5),
                ItemTestData.CreateRegularItem(id: 6),
                ItemTestData.CreateRegularItem(id: 6),
                ItemTestData.CreateCumulativeItem(id: 7, amount: 100),
                ItemTestData.CreateCumulativeItem(id: 7, amount: 15),
                ItemTestData.CreateCumulativeItem(id:8, amount: 3),
            };

            var top1Expected = new List<IItem>
            {
                ItemTestData.CreateTopItem(id: 2, topOrder: 1),
            };
            var top2Expected = new List<IItem>
            {
                ItemTestData.CreateTopItem(id: 3, topOrder: 2),
                ItemTestData.CreateTopItem(id: 4, topOrder: 2),
            };

            Assert.Collection(tile.TopItems, item => Assert.Equal(top1Expected[0].ClientId, item));

            Assert.Collection(tile.DownItems, item => { Assert.Equal(downItemsExpected[5].ClientId, item.ClientId); Assert.Equal((downItemsExpected[5] as ICumulativeItem).Amount, (item as ICumulativeItem).Amount); },
                                              item => { Assert.Equal(downItemsExpected[4].ClientId, item.ClientId); Assert.Equal((downItemsExpected[4] as ICumulativeItem).Amount, (item as ICumulativeItem).Amount); },
                                              item => { Assert.Equal(downItemsExpected[3].ClientId, item.ClientId); Assert.Equal((downItemsExpected[3] as ICumulativeItem).Amount, (item as ICumulativeItem).Amount); },
                                              item => Assert.Equal(downItemsExpected[2].ClientId, item.ClientId),
                                              item => Assert.Equal(downItemsExpected[1].ClientId, item.ClientId),
                                              item => Assert.Equal(downItemsExpected[0].ClientId, item.ClientId));

        }

        [Fact]
        public void RemoveThing_Removes_Item_From_Stack()
        {
            var item = ItemTestData.CreateMoveableItem(500);
            var sut = CreateTile(item);

            var thing = (IMoveableThing)item;
            sut.RemoveThing(ref thing);

            Assert.Equal(2, sut.DownItems.Count);
            Assert.Single(sut.TopItems);

            Assert.Equal(200, sut.DownItems.First().ClientId);
        }
        [Theory]
        [ClassData(typeof(RemoveThingTileTestData))]
        public void RemoveThing_Removes_CumulativeItem_From_Stack(ICumulativeItem item, byte amountToRemove, ushort topItemId, byte remainingAmount)
        {
            var item2 = ItemTestData.CreateCumulativeItem(400, 32);
            var sut = CreateTile(item2, item);

            var thing = (IMoveableThing)item;
            sut.RemoveThing(ref thing, amountToRemove);

            Assert.Equal(topItemId, sut.DownItems.First().ClientId);
            Assert.Equal(remainingAmount, (sut.DownItems.First() as ICumulativeItem).Amount);
        }

        [Fact]
        public void AddThing_When_Cumulative_On_Top_Join_If_Same_Type()
        {
            var item = ItemTestData.CreateThrowableDistanceItem(500, 5);
            var sut = CreateTile(item);

            var item2 = ItemTestData.CreateThrowableDistanceItem(500, 3) as IMoveableThing;
            sut.AddThing(ref item2);

            Assert.Equal(3, sut.DownItems.Count);
            Assert.Single(sut.TopItems);

            Assert.Equal(500, sut.DownItems.First().ClientId);
            Assert.Equal(8, (sut.DownItems.First() as ICumulativeItem).Amount);
        }

        [Theory]
        [MemberData(nameof(NextTilesTestData))]
        public void IsNextTo_When_1_Sqm_Distant_Returns_True(ITile dest)
        {
            ITile sut = new Tile(new Coordinate(100, 100, 7), TileFlag.None, null, new IItem[0], new IItem[0]);

            Assert.True(sut.IsNextTo(dest));
        }

        [Fact]
        public void IsNextTo_When_2_Or_More_Sqm_Distant_Returns_True()
        {
            ITile sut = new Tile(new Coordinate(100, 100, 7), TileFlag.None, null, new IItem[0], new IItem[0]);
            ITile dest = new Tile(new Coordinate(102, 100, 7), TileFlag.None, null, new IItem[0], new IItem[0]);

            Assert.False(sut.IsNextTo(dest));
        }
    }

}
