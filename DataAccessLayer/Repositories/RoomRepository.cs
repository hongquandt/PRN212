using BusinessObjects;
using DataAccessLayer.DataContext;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly InMemoryDataContext _context;

        public RoomRepository()
        {
            _context = InMemoryDataContext.Instance;
        }

        public List<RoomInformation> GetAllRooms()
        {
            return _context.Rooms.Where(r => r.RoomStatus == 1).ToList();
        }

        public RoomInformation? GetRoomById(int id)
        {
            return _context.Rooms.FirstOrDefault(r => r.RoomID == id && r.RoomStatus == 1);
        }

        public void AddRoom(RoomInformation room)
        {
            room.RoomID = _context.GetNextRoomId();
            room.RoomStatus = 1;
            room.RoomType = _context.RoomTypes.FirstOrDefault(rt => rt.RoomTypeID == room.RoomTypeID);
            _context.Rooms.Add(room);
        }

        public void UpdateRoom(RoomInformation room)
        {
            var existingRoom = _context.Rooms.FirstOrDefault(r => r.RoomID == room.RoomID);
            if (existingRoom != null)
            {
                existingRoom.RoomNumber = room.RoomNumber;
                existingRoom.RoomDescription = room.RoomDescription;
                existingRoom.RoomMaxCapacity = room.RoomMaxCapacity;
                existingRoom.RoomPricePerDate = room.RoomPricePerDate;
                existingRoom.RoomTypeID = room.RoomTypeID;
                existingRoom.RoomType = _context.RoomTypes.FirstOrDefault(rt => rt.RoomTypeID == room.RoomTypeID);
            }
        }

        public void DeleteRoom(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.RoomID == id);
            if (room != null)
            {
                room.RoomStatus = 2; // Soft delete
            }
        }

        public List<RoomInformation> SearchRooms(string searchTerm)
        {
            return _context.Rooms
                .Where(r => r.RoomStatus == 1 &&
                           (r.RoomNumber.Contains(searchTerm) ||
                            r.RoomDescription.Contains(searchTerm)))
                .ToList();
        }

        public List<RoomType> GetAllRoomTypes()
        {
            return _context.RoomTypes.ToList();
        }
    }
}
