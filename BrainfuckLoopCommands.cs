using System.Collections.Generic;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        public static void RegisterTo(IVirtualMachine vm)
        {
            //Ради интереса написал через один словарь :)
            var openClosedBracketsPairs = new Dictionary<int, (int, int)>();
            
            FillDictionariesOfBracketsPairs(vm.Instructions, openClosedBracketsPairs);
            
            vm.RegisterCommand('[', b =>
            {
                if (b.Memory[b.MemoryPointer] == 0)
                    b.InstructionPointer = openClosedBracketsPairs[b.InstructionPointer].Item2;
            });

            vm.RegisterCommand(']', b =>
            {
                if (b.Memory[b.MemoryPointer] != 0)
                    b.InstructionPointer = openClosedBracketsPairs[b.InstructionPointer].Item1;
            });
        }

        private static void FillDictionariesOfBracketsPairs(
            string instructions, 
            Dictionary<int, (int, int)> openClosedBracketsPairs)
        {
            var positionsOfOpenBrackets = new Stack<int>();
            for (var instructionPointer = 0; instructionPointer < instructions.Length; instructionPointer++)
            {
                switch (instructions[instructionPointer])
                {
                    case '[':
                        positionsOfOpenBrackets.Push(instructionPointer);
                        break;
                    case ']':
                        var lastOpenBracketPosition = positionsOfOpenBrackets.Pop();
                        var openClosedBracketsPair = (lastOpenBracketPosition, instructionPointer);
                        openClosedBracketsPairs.Add(lastOpenBracketPosition, openClosedBracketsPair);
                        openClosedBracketsPairs.Add(instructionPointer, openClosedBracketsPair);
                        break;
                }
            }
        }
    }
}