using BusinessObjects;
using System.Collections.Generic;

namespace DataAccessLayer.Repositories
{
    public interface IRoomRepository
    {
        List<RoomInformation> GetAllRooms();
        RoomInformation? GetRoomById(int id);
        void AddRoom(RoomInformation room);
        void UpdateRoom(RoomInformation room);
        void DeleteRoom(int id);
        List<RoomInformation> SearchRooms(string searchTerm);
        List<RoomType> GetAllRoomTypes();
    }
}
