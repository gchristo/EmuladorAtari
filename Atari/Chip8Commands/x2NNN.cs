namespace Emulator.Chip8Commands
{
    /// <summary>
    //Calls subroutine at NNN.
    /// </summary>
    public class x2NNN : BaseCommand
    {
        ushort address;
        public x2NNN(ushort opcode, Chip8 chip) : base(chip)
        {
            address = (ushort)(opcode & 0x0FFF);
        }

        public override void Execute()
        {
            Stack[SP] = PC;         // Store current address in stack
            ++SP;                   // Increment stack pointer
            PC = address;   // Set the program counter to the address at NNN
        }
    }
}
