using BusinessObjects;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService()
        {
            _roomRepository = new RoomRepository();
        }

        public List<RoomInformation> GetAllRooms()
        {
            return _roomRepository.GetAllRooms();
        }

        public RoomInformation? GetRoomById(int id)
        {
            return _roomRepository.GetRoomById(id);
        }

        public void AddRoom(RoomInformation room)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(room.RoomNumber))
                throw new ArgumentException("Room number is required");

            if (room.RoomMaxCapacity <= 0)
                throw new ArgumentException("Room capacity must be greater than 0");

            if (room.RoomPricePerDate <= 0)
                throw new ArgumentException("Room price must be greater than 0");

            _roomRepository.AddRoom(room);
        }

        public void UpdateRoom(RoomInformation room)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(room.RoomNumber))
                throw new ArgumentException("Room number is required");

            if (room.RoomMaxCapacity <= 0)
                throw new ArgumentException("Room capacity must be greater than 0");

            if (room.RoomPricePerDate <= 0)
                throw new ArgumentException("Room price must be greater than 0");

            _roomRepository.UpdateRoom(room);
        }

        public void DeleteRoom(int id)
        {
            _roomRepository.DeleteRoom(id);
        }

        public List<RoomInformation> SearchRooms(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return GetAllRooms();

            return _roomRepository.SearchRooms(searchTerm);
        }

        public List<RoomType> GetAllRoomTypes()
        {
            return _roomRepository.GetAllRoomTypes();
        }
    }
}
