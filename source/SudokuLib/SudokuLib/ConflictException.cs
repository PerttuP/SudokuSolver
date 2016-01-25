using System;
using System.Runtime.Serialization;

namespace SudokuLib
{
    /// <summary>
    /// Exception class to signal detected conflicts in sudoku table.
    /// </summary>
    public class ConflictException : Exception
    {
        public ConflictException(): base() { }
        public ConflictException(String message): base(message) { }
        public ConflictException(String message, Exception innerException): base(message, innerException) { }
        public ConflictException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
