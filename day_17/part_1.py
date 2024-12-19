import pytest

from enum import IntEnum
from pathlib import Path


class OpCode(IntEnum):
    ADV = 0
    BXL = 1
    BST = 2
    JNZ = 3
    BXC = 4
    OUT = 5
    BDV = 6
    CDV = 7


class Computer:

    def __init__(self, reg_a, reg_b, reg_c, program):
        self.reg_a = reg_a
        self.reg_b = reg_b
        self.reg_c = reg_c
        self.program = program
        self.pc = 0
        self._output = []

    @property
    def output(self):
        return ",".join([str(i) for i in self._output])

    def combo(self, value: int) -> int:
        if 0 <= value <= 3:
            return value
        elif value == 4:
            return self.reg_a
        elif value == 5:
            return self.reg_b
        elif value == 6:
            return self.reg_c
        else:
            raise RuntimeError("Invalid program.")

    def adv(self, operand: int) -> None:
        self.reg_a = self.reg_a >> self.combo(operand)

    def bxl(self, operand: int) -> None:
        self.reg_b = self.reg_b ^ operand

    def bst(self, operand: int) -> None:
        self.reg_b = (self.combo(operand) % 8) & 0b111

    def jnz(self, operand: int) -> None:
        if self.reg_a == 0:
            return
        self.pc = operand

    def bxc(self, operand: int) -> None:
        self.reg_b = self.reg_b ^ self.reg_c

    def out(self, operand: int) -> None:
        self._output.append(self.combo(operand) % 8)

    def bdv(self, operand: int) -> None:
        self.reg_b = self.reg_a >> self.combo(operand)
    
    def cdv(self, operand: int) -> None:
        self.reg_c = self.reg_a >> self.combo(operand)

    def run(self, debug: bool = False):
        if debug:
            print(f"op=INI  reg_a={bin(self.reg_a)}  reg_b={bin(self.reg_b)}  reg_c={bin(self.reg_c)}  output={self.output}")
        while self.pc < len(self.program) - 1:
            operand = self.program[self.pc + 1]
            initial_pc = self.pc
            opcode = self.program[self.pc]
            match opcode:
                case OpCode.ADV:
                    self.adv(operand)
                case OpCode.BXL:
                    self.bxl(operand)
                case OpCode.BST:
                    self.bst(operand)
                case OpCode.JNZ:
                    self.jnz(operand)
                case OpCode.BXC:
                    self.bxc(operand)
                case OpCode.OUT:
                    self.out(operand)
                case OpCode.BDV:
                    self.bdv(operand)
                case OpCode.CDV:
                    self.cdv(operand)
                case _:
                    raise RuntimeError(f"Invalid opcode {opcode}.")
            if self.pc == initial_pc:
                self.pc += 2
            if debug:
                print(f"op={OpCode(opcode).name}  reg_a={bin(self.reg_a)}  reg_b={bin(self.reg_b)}  reg_c={bin(self.reg_c)}  output={self.output}")

@pytest.mark.parametrize(("computer", "reg_a", "reg_b", "output"), [
    (Computer(reg_a=0, reg_b=0, reg_c=9, program=[2,6]), None, 1, None),
    (Computer(reg_a=0, reg_b=29, reg_c=0, program=[1,7]), None, 26, None),
    (Computer(reg_a=0, reg_b=2024, reg_c=43690, program=[4,0]), None, 44354, None),
    (Computer(reg_a=10, reg_b=0, reg_c=0, program=[5,0,5,1,5,4]), None, None, "0,1,2"),
    (Computer(reg_a=2024, reg_b=0, reg_c=0, program=[0,1,5,4,3,0]), 0, None, "4,2,5,6,7,7,7,7,3,1,0"),
])
def test_computer(computer, reg_a, reg_b, output):
    computer.run()
    if reg_a:
        assert computer.reg_a == reg_a
    if reg_b:
        assert computer.reg_b == reg_b
    if output:
        assert computer.output == output


def parse_input(input_path: Path) -> tuple[int, int, int, list[str]]:
    reg_a = None
    reg_b = None
    reg_c = None
    program = None
    with input_path.open() as file:
        for line in file.readlines():
            if line.startswith("Register A: "):
                reg_a = int(line.removeprefix("Register A: "))
            elif line.startswith("Register B: "):
                reg_b = int(line.removeprefix("Register B: "))
            elif line.startswith("Register C: "):
                reg_c = int(line.removeprefix("Register C: "))
            elif line.startswith("Program: "):
                program = [int(i) for i in line.removeprefix("Program: ").split(",")]
    return reg_a, reg_b, reg_c, program


def solver(output: list[int]) -> list[int]:
    if not output:
        return [0]
    
    sol = []
    for prev in solver(output[1:]):
        for num in range(8):
            num = (prev << 3) | num
            computer = Computer(reg_a=num, reg_b=0, reg_c=0, program=program)
            computer.run()
            if computer.output == ",".join([str(i) for i in output]):
                sol.append(num)
    return sol


if __name__ == "__main__":
    input_path = Path("../data/day_17/input.txt")
    _, _, _, program = parse_input(input_path)
    print(f"Result: {min(solver(program))}")
