import sys

from pathlib import Path


sys.setrecursionlimit(100000)


def main(input_path: Path) -> int:
    map = load_map_from_file(input_path)
    memory = {}
    score = find_path_lowest_score(map, (len(map) - 2, 1), (0, 1), memory=memory)
    print(len(memory.keys()))
    return score


def load_map_from_file(input_path: Path) -> list[list[str]]:
    map = []
    with input_path.open() as file:
        for line in file.readlines():
            map.append(line[:-1])
    return map


def find_path_lowest_score(map: list[list[str]], position: tuple[int, int], direction: tuple[int, int], path: list[tuple[int, int]] = [], *, memory: dict[tuple[int, int], int]) -> int | None:
    if position == (1, len(map[0]) - 2):
        return 0

    if position in path:
        return None

    if (position, direction) in memory.keys():
        return memory[(position, direction)]

    lowest_score = None
    num_free_neighboring_tiles = 0
    for next_position, next_direction in get_free_neighboring_tiles(map, position, direction):
        num_free_neighboring_tiles += 1
        score = find_path_lowest_score(map, next_position, next_direction, path + [position], memory=memory)
        if score is not None:
            score += 1 if next_direction == direction else 1001
            if lowest_score is None or score < lowest_score:
                lowest_score = score
    if num_free_neighboring_tiles > 1:
        memory[(position, direction)] = lowest_score
    return lowest_score


def get_free_neighboring_tiles(map: list[list[str]], position: tuple[int, int], direction: tuple[int, int]):
    for move in {(-1, 0), (0, 1), (1, 0), (0, -1)} - {(-direction[0], -direction[1])}:
        new_position = (position[0] + move[0], position[1] + move[1])
        if 0 <= new_position[0] < len(map) and 0<= position[1] < len(map[0]) and map[new_position[0]][new_position[1]] != "#":
            yield new_position, move


if __name__ == "__main__":
    print(f"Score: {main(Path('../data/day_16/sample2.txt'))}")
