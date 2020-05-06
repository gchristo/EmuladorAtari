namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Returns from subroutine
    /// </summary>
    public class x00EE : BaseCommand
    {
        public x00EE(Chip8 chip) : base(chip) { }

        public override void Execute()
        {
            --SP;           // 16 levels of stack, decrease stack pointer to prevent overwrite
            PC = Stack[SP]; // Put the stored return address from the stack back into the program counter					
            PC += 2;        // Don't forget to increase the program counter!
        }
    }
}
