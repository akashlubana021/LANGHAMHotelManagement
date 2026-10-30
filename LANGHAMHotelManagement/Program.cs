
using System;
using System.Collections.Generic;
using System.IO;

namespace Assessment2Task2
{
    public class Room
    {
        public int RoomNo { get; set; }
        public bool IsAllocated { get; set; }
    }

    public class Customer
    {
        public int CustomerNo { get; set; }
        public string CustomerName { get; set; } = "Karan";
    }

    public class RoomAllocation
    {
        public int AllocatedRoomNo { get; set; }
        public Customer AllocatedCustomer { get; set; } = new Customer();
    }

    class Program
    {
        static Room[] listOfRooms = Array.Empty<Room>();

        static List<RoomAllocation> listOfRoomAllocations =
            new List<RoomAllocation>();

        // Replace studentid with your actual student ID.
        static readonly string fileName = "lhms_850010451.txt";

        static void Main(string[] args)
        {
            int choice = 0;

            while (choice != 9)
            {
                Console.WriteLine("\n****************************************");
                Console.WriteLine("     LANGHAM HOTEL MANAGEMENT SYSTEM");
                Console.WriteLine("                 MENU");
                Console.WriteLine("****************************************");
                Console.WriteLine("1. Add Rooms");
                Console.WriteLine("2. Display Rooms");
                Console.WriteLine("3. Allocate Rooms");
                Console.WriteLine("4. De-Allocate Rooms");
                Console.WriteLine("5. Display Room Allocation Details");
                Console.WriteLine("6. Billing");
                Console.WriteLine("7. Save the Room Allocations To a File");
                Console.WriteLine("8. Show the Room Allocations From a File");
                Console.WriteLine("9. Exit");
                Console.WriteLine("10. Backup File");
                Console.WriteLine("11. Exception Handling Demo");
                Console.WriteLine("****************************************");

                Console.Write("Enter your choice: ");

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Please enter a valid number.");
                    choice = 0;
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        AddRooms();
                        break;

                    case 2:
                        DisplayRooms();
                        break;

                    case 3:
                        AllocateRoom();
                        break;

                    case 4:
                        DeAllocateRoom();
                        break;

                    case 5:
                        DisplayRoomAllocationDetails();
                        break;

                    case 6:
                        Console.WriteLine(
                            "Billing Feature is Under Construction and will be added soon…!!!");
                        break;

                    case 7:
                        SaveAllocationsToFile();
                        break;

                    case 8:
                        ShowAllocationsFromFile();
                        break;

                    case 9:
                        Console.WriteLine("Exiting the application. Goodbye!");
                        break;

                    case 10:
                        BackupFile();
                        break;

                    case 11:
                        DemonstrateExceptions();
                        break;

                    default:
                        Console.WriteLine("This option is not implemented yet.");
                        break;
                }
            }
        }

        // TASK 2: ADD ROOMS
        static void AddRooms()
        {
            Console.Write("How many rooms would you like to add? ");

            if (!int.TryParse(Console.ReadLine(), out int numberOfRooms)
                || numberOfRooms <= 0)
            {
                Console.WriteLine(
                    "Please enter a whole number greater than zero.");
                return;
            }

            Room[] newRooms = new Room[numberOfRooms];

            for (int i = 0; i < numberOfRooms; i++)
            {
                Console.Write($"Enter room number {i + 1}: ");

                if (!int.TryParse(Console.ReadLine(), out int roomNumber)
                    || roomNumber <= 0)
                {
                    Console.WriteLine(
                        "Invalid room number. Please try adding rooms again.");
                    return;
                }

                foreach (Room existingRoom in listOfRooms)
                {
                    if (existingRoom.RoomNo == roomNumber)
                    {
                        Console.WriteLine(
                            $"Room {roomNumber} already exists. Please try again.");
                        return;
                    }
                }

                for (int j = 0; j < i; j++)
                {
                    if (newRooms[j].RoomNo == roomNumber)
                    {
                        Console.WriteLine(
                            $"Room {roomNumber} was entered twice. Please try again.");
                        return;
                    }
                }

                newRooms[i] = new Room
                {
                    RoomNo = roomNumber,
                    IsAllocated = false
                };
            }

            Room[] combinedRooms =
                new Room[listOfRooms.Length + newRooms.Length];

            Array.Copy(listOfRooms, combinedRooms, listOfRooms.Length);

            Array.Copy(
                newRooms, 0, combinedRooms,
                listOfRooms.Length, newRooms.Length);

            listOfRooms = combinedRooms;

            Console.WriteLine(
                $"{numberOfRooms} room(s) added successfully.");

            LogActivity($"{numberOfRooms} room(s) added.");
        }

        // TASK 2: DISPLAY ROOMS
        static void DisplayRooms()
        {
            Console.WriteLine("\nROOM LIST");

            if (listOfRooms.Length == 0)
            {
                Console.WriteLine("No rooms have been added yet.");
                return;
            }

            Console.WriteLine("Room No.\tAllocation Status");

            foreach (Room room in listOfRooms)
            {
                string status = room.IsAllocated
                    ? "Allocated"
                    : "Available";

                Console.WriteLine($"{room.RoomNo}\t\t{status}");
            }
        }

        // TASK 2: ALLOCATE ROOMS
        static void AllocateRoom()
        {
            if (listOfRooms.Length == 0)
            {
                Console.WriteLine(
                    "Please add rooms before allocating a room.");
                return;
            }

            Console.WriteLine("\nAVAILABLE ROOMS");

            bool availableRoomFound = false;

            foreach (Room room in listOfRooms)
            {
                if (!room.IsAllocated)
                {
                    Console.WriteLine($"Room {room.RoomNo}");
                    availableRoomFound = true;
                }
            }

            if (!availableRoomFound)
            {
                Console.WriteLine("There are no available rooms.");
                return;
            }

            Console.Write("Enter the room number to allocate: ");

            if (!int.TryParse(Console.ReadLine(), out int roomNumber))
            {
                Console.WriteLine("Please enter a valid room number.");
                return;
            }

            Room? selectedRoom = null;

            foreach (Room room in listOfRooms)
            {
                if (room.RoomNo == roomNumber)
                {
                    selectedRoom = room;
                    break;
                }
            }

            if (selectedRoom == null)
            {
                Console.WriteLine("That room does not exist.");
                return;
            }

            if (selectedRoom.IsAllocated)
            {
                Console.WriteLine("That room is already allocated.");
                return;
            }

            Console.Write("Enter the customer number: ");

            if (!int.TryParse(Console.ReadLine(), out int customerNumber)
                || customerNumber <= 0)
            {
                Console.WriteLine("Please enter a valid customer number.");
                return;
            }

            Console.Write("Enter the customer name: ");
            string customerName = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(customerName))
            {
                Console.WriteLine("Customer name cannot be empty.");
                return;
            }

            Customer customer = new Customer
            {
                CustomerNo = customerNumber,
                CustomerName = customerName
            };

            RoomAllocation allocation = new RoomAllocation
            {
                AllocatedRoomNo = selectedRoom.RoomNo,
                AllocatedCustomer = customer
            };

            selectedRoom.IsAllocated = true;
            listOfRoomAllocations.Add(allocation);

            Console.WriteLine(
                $"Room {selectedRoom.RoomNo} has been allocated successfully to {customerName}.");

            LogActivity(
                $"Room {selectedRoom.RoomNo} allocated to customer " +
                $"{customer.CustomerNo} ({customer.CustomerName}).");
        }

        // TASK 2: DE-ALLOCATE ROOMS
        static void DeAllocateRoom()
        {
            Console.WriteLine("\nALLOCATED ROOMS");

            bool allocatedRoomFound = false;

            foreach (Room room in listOfRooms)
            {
                if (room.IsAllocated)
                {
                    Console.WriteLine($"Room {room.RoomNo}");
                    allocatedRoomFound = true;
                }
            }

            if (!allocatedRoomFound)
            {
                Console.WriteLine(
                    "There are no allocated rooms to release.");
                return;
            }

            Console.Write("Enter the room number to de-allocate: ");

            if (!int.TryParse(Console.ReadLine(), out int roomNumber))
            {
                Console.WriteLine("Please enter a valid room number.");
                return;
            }

            Room? selectedRoom = null;

            foreach (Room room in listOfRooms)
            {
                if (room.RoomNo == roomNumber)
                {
                    selectedRoom = room;
                    break;
                }
            }

            if (selectedRoom == null)
            {
                Console.WriteLine("That room does not exist.");
                return;
            }

            if (!selectedRoom.IsAllocated)
            {
                Console.WriteLine(
                    "That room is not currently allocated.");
                return;
            }

            RoomAllocation? previousAllocation =
                listOfRoomAllocations.Find(
                    allocation => allocation.AllocatedRoomNo == roomNumber);

            selectedRoom.IsAllocated = false;

            listOfRoomAllocations.RemoveAll(
                allocation => allocation.AllocatedRoomNo == roomNumber);

            Console.WriteLine(
                $"Room {roomNumber} has been de-allocated successfully.");

            string customerDetails = previousAllocation == null
                ? ""
                : $" from customer {previousAllocation.AllocatedCustomer.CustomerName}";

            LogActivity($"Room {roomNumber} de-allocated{customerDetails}.");
        }

        // TASK 2: DISPLAY ALLOCATION DETAILS
        static void DisplayRoomAllocationDetails()
        {
            Console.WriteLine("\nROOM ALLOCATION DETAILS");

            if (listOfRoomAllocations.Count == 0)
            {
                Console.WriteLine(
                    "There are no current room allocations.");
                return;
            }

            Console.WriteLine("Room No.\tCustomer No.\tCustomer Name");

            foreach (RoomAllocation allocation in listOfRoomAllocations)
            {
                Console.WriteLine(
                    $"{allocation.AllocatedRoomNo}\t\t" +
                    $"{allocation.AllocatedCustomer.CustomerNo}\t\t" +
                    $"{allocation.AllocatedCustomer.CustomerName}");
            }
        }

        // TASK 2: RECORD APPLICATION ACTIVITY
        static void LogActivity(string message)
        {
            try
            {
                string record =
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}" +
                    Environment.NewLine;

                File.AppendAllText(fileName, record);
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine(
                    "Activity could not be saved because file access was denied.");
            }
            catch (IOException ex)
            {
                Console.WriteLine(
                    $"File error while recording activity: {ex.Message}");
            }
        }

        // TASK 2: SAVE ALLOCATIONS TO FILE
        static void SaveAllocationsToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName, true))
                {
                    writer.WriteLine(
                        $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Manual save requested.");

                    if (listOfRoomAllocations.Count == 0)
                    {
                        writer.WriteLine("No current room allocations.");
                    }
                    else
                    {
                        foreach (RoomAllocation allocation in listOfRoomAllocations)
                        {
                            writer.WriteLine(
                                $"Room {allocation.AllocatedRoomNo}, " +
                                $"Customer {allocation.AllocatedCustomer.CustomerNo}, " +
                                $"Name: {allocation.AllocatedCustomer.CustomerName}");
                        }
                    }
                }

                Console.WriteLine(
                    $"Room allocation records saved to {fileName}.");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access denied: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"File error while saving: {ex.Message}");
            }
        }

        // TASK 2: READ ALLOCATIONS FROM FILE
        static void ShowAllocationsFromFile()
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    Console.WriteLine(
                        $"The file {fileName} does not exist yet. Choose Option 7 first.");
                    return;
                }

                using (StreamReader reader = new StreamReader(fileName))
                {
                    Console.WriteLine("\nSAVED FILE CONTENTS");

                    string? line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access denied: {ex.Message}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"File error while reading: {ex.Message}");
            }
        }

        // TASK 2: BACKUP FILE
        static void BackupFile()
        {
            string backupFileName = "lhms_850010451_backup.txt";

            try
            {
                if (!File.Exists(fileName))
                {
                    Console.WriteLine(
                        $"The original file {fileName} does not exist. Nothing to back up.");
                    return;
                }

                string originalContent = File.ReadAllText(fileName);

                if (string.IsNullOrEmpty(originalContent))
                {
                    Console.WriteLine(
                        "The original file is empty. Nothing to back up.");
                    return;
                }

                File.AppendAllText(backupFileName, originalContent);

                // Clear the original only after the backup write succeeds.
                File.WriteAllText(fileName, string.Empty);

                Console.WriteLine(
                    $"Backup completed successfully: {backupFileName}");

                Console.WriteLine(
                    $"The original file {fileName} has been cleared.");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access denied: {ex.Message}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine(
                    $"File error during backup: {ex.Message}");
            }
        }

        // TASK 3: RUN ALL FOUR EXCEPTION DEMONSTRATIONS
        static void DemonstrateExceptions()
        {
            Console.WriteLine("\n========================================");
            Console.WriteLine("       EXCEPTION HANDLING DEMO");
            Console.WriteLine("========================================");

            DemonstrateFormatException();
            DemonstrateInvalidOperationException();
            DemonstrateFileNotFoundException();
            DemonstrateUnauthorizedAccessException();

            Console.WriteLine("\nAll exception demonstrations completed.");
        }

        // TASK 3(a): FORMAT EXCEPTION
        static void DemonstrateFormatException()
        {
            Console.WriteLine("\n1. FORMAT EXCEPTION");

            try
            {
                Console.Write("Enter a room number using letters (e.g. ABC): ");

                string enteredValue = Console.ReadLine() ?? "";

                int roomNumber = int.Parse(enteredValue);

                Console.WriteLine($"Room number entered: {roomNumber}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine("FormatException caught successfully.");
                Console.WriteLine(
                    "The entered value has an incorrect data type.");
                Console.WriteLine($"Details: {ex.Message}");
            }
            finally
            {
                Console.WriteLine(
                    "FormatException demonstration completed.");
            }
        }

        // TASK 3(b): INVALID OPERATION EXCEPTION
        static void DemonstrateInvalidOperationException()
        {
            Console.WriteLine("\n2. INVALID OPERATION EXCEPTION");

            List<int> availableRooms = new List<int> { 101, 102, 103 };

            try
            {
                Console.WriteLine("Available room numbers: 101, 102, 103");
                Console.Write("Enter a room number: ");

                string enteredValue = Console.ReadLine() ?? "";

                if (!int.TryParse(enteredValue, out int roomNumber))
                {
                    Console.WriteLine(
                        "Please enter a valid numeric room number.");
                    return;
                }

                if (!availableRooms.Contains(roomNumber))
                {
                    throw new InvalidOperationException(
                        $"Room {roomNumber} does not exist in the available room list.");
                }

                Console.WriteLine(
                    $"Room {roomNumber} exists in the available room list.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(
                    "InvalidOperationException caught successfully.");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine(
                    "InvalidOperationException demonstration completed.");
            }
        }

        // TASK 3(c): FILE NOT FOUND EXCEPTION
        static void DemonstrateFileNotFoundException()
        {
            Console.WriteLine("\n3. FILE NOT FOUND EXCEPTION");

            try
            {
                string missingFile = Path.Combine(
                    Path.GetTempPath(),
                    "LHMS_File_That_Does_Not_Exist.txt");

                using (FileStream stream = File.OpenRead(missingFile))
                {
                    Console.WriteLine("File opened successfully.");
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(
                    "FileNotFoundException caught successfully.");
                Console.WriteLine(
                    "The requested file could not be found.");
                Console.WriteLine($"Details: {ex.Message}");
            }
            finally
            {
                Console.WriteLine(
                    "FileNotFoundException demonstration completed.");
            }
        }

        // TASK 3(d): UNAUTHORIZED ACCESS EXCEPTION
        static void DemonstrateUnauthorizedAccessException()
        {
            Console.WriteLine("\n4. UNAUTHORIZED ACCESS EXCEPTION");

            string testFile = Path.Combine(
                Path.GetTempPath(),
                "LHMS_ReadOnly_Test.txt");

            bool fileCreated = false;
            bool readOnlySet = false;

            try
            {
                // Create a temporary file for the test.
                File.WriteAllText(
                    testFile,
                    "This file is used to demonstrate read-only access.");

                fileCreated = true;

                // Change the file to read-only.
                File.SetAttributes(
                    testFile,
                    File.GetAttributes(testFile) | FileAttributes.ReadOnly);

                readOnlySet = true;

                Console.WriteLine(
                    "The test file has been set to read-only.");

                Console.WriteLine(
                    "The program will now attempt to write to this file.");

                // Attempting to write to a read-only file on Windows
                // should cause UnauthorizedAccessException.
                File.WriteAllText(
                    testFile,
                    "Attempting to overwrite the read-only file.");

                Console.WriteLine(
                    "The file was written successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(
                    "UnauthorizedAccessException caught successfully.");

                Console.WriteLine(
                    "The program cannot write to a read-only file.");

                Console.WriteLine($"Details: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine(
                    $"An I/O error occurred during the test: {ex.Message}");
            }
            finally
            {
                // Restore normal permissions before deleting the test file.
                try
                {
                    if (fileCreated && File.Exists(testFile))
                    {
                        if (readOnlySet)
                        {
                            File.SetAttributes(
                                testFile,
                                FileAttributes.Normal);
                        }

                        File.Delete(testFile);

                        Console.WriteLine(
                            "Temporary test file cleaned up successfully.");
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine(
                        $"Could not clean up the test file: {ex.Message}");
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine(
                        $"Could not restore file access: {ex.Message}");
                }

                Console.WriteLine(
                    "UnauthorizedAccessException demonstration completed.");
            }
        }
    }
}
