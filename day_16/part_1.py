import heapq

from collections import defaultdict
from dataclasses import dataclass
from pathlib import Path


@dataclass
class Node:
    position: tuple[int, int]
    direction: tuple[int, int]


class Graph:

    def __init__(self, maze: list[list[str]], start: tuple[int, int], end: tuple[int, int]) -> None:
        self.maze = maze
        self.start = start
        self.end = end
        self.path_scores = {}

    @classmethod
    def load_from_file(cls, input_path: Path) -> "Graph":
        maze = []
        with input_path.open() as file:
            for line in file.readlines():
                maze.append(line[:-1])
        return cls(
            maze=maze,
            start=(len(maze) - 2, 1),
            end=(1, len(maze[0]) - 2),
        )

    @property
    def source_vertex(self) -> Node:
        return Node(*self.next_fork(self.start, (0, 1)))

    @property
    def final_vertex(self) -> tuple[int, int]:
        if self.is_fork(self.end):
            return self.end
        return self.next_fork(self.end)

    @property
    def initial_score(self) -> int:
        return self.path_scores(self.start, self.source_vertex[0], (0, 1))

    def final_score(self, direction: tuple[int, int]) -> int:
        return self.path_scores(self.final_vertex, self.end, direction)

    def neighbors(self, v: tuple[int, int], direction: tuple[int, int]) -> list[tuple[int, int]]:
        return [self.next_fork(v, direction) for direction in self.valid_directions(v)]

    def next_fork(self, v: Node) -> Node:
        initial = v
        score = 0
        while not self.is_fork(v):
            next_direction = self.get_valid_directions(v)
            if len(next_direction) > 1:
                raise RuntimeError()
            score += 1 if v.direction == next_direction else 1001
            v.position = (v.position[0] + next_direction[0], v.position[1] + next_direction[1])
            v.direction = next_direction
        self.path_scores[initial, v] = score
        return v

    def get_valid_directions(self, v: Node) -> list[tuple[int, int]]:
        directions = {(1, 0), (-1, 0), (0, 1), (0, -1)} - {(-initial_direction[0], -initial_direction[1])} if initial_direction is not None else {}
        directions = {direction: (position[0] + direction[0], position[1] + direction[1]) for direction in directions}
        directions = {direction: next_position for direction, next_position in directions.items() if self.is_within_borders(next_position) and self.maze[next_position[0]][next_position[1]] != "#"}
        return [*directions.keys()]

    def is_fork(self, v: tuple[int, int]) -> bool:
        num_neighbouring_free_tiles = 0
        for tile in [(v[0] + move[0], v[1] + move[1]) for move in {(1, 0), (-1, 0), (0, 1), (0, -1)}]:
            if self.is_within_borders(tile) and self.maze[tile[0]][tile[1]] != "%":
                neighbouring_free_tiles.append(tile)
        return len(neighbouring_free_tiles) > 2

    def is_within_borders(self, v: tuple[int, int]) -> bool:
        if 1 <= v[0] < len(self.maze) - 1 and 1 <= v[1] < len(self.maze[0]) - 1:
            return True
        return False


def main(input_path: Path) -> int:
    graph = Graph.load_from_file(input_path)
    dist = defaultdict(lambda: float("inf"))
    visited = defaultdict(bool)
    priority_queue = []

    source, direction, score = graph.source_vertex
    dist[(source, direction)] = score
    heapq.heappush(priority_queue, (score, source, direction))

    while priority_queue:
        _, u, direction = heapq.heappop(priority_queue)

        if visited[u]:
            continue
        visited[u] = True

        for neighbor in graph.neighbors(u, direction):
            if not visited[neighbor]:
                alt = dist[(u, direction)] + graph.path_scores(u, neighbor)
                if alt < dist[neighbor]:
                    dist[neighbor] = alt
                    heapq.heappush(priority_queue, (dist[neighbor], neighbor, direction))

    return dist[graph.final_vertex] + graph.final_score


if __name__ == "__main__":
    print(f"Score: {main(Path('../data/day_16/sample2.txt'))}")
