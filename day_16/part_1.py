import heapq

from collections import defaultdict
from dataclasses import dataclass
from pathlib import Path


@dataclass
class Node:
    position: tuple[int, int]
    direction: tuple[int, int]

    def __hash__(self):
        return hash(self.position) + hash(self.direction)

    def __gt__(self, other: "Node") -> bool:
        return True


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
        return Node(self.start, (0, 1))

    def neighbors(self, v: Node) -> list[Node]:
        directions = {(1, 0), (-1, 0), (0, 1), (0, -1)} - {(-v.direction[0], -v.direction[1])}
        neighbors = set()
        for direction in directions:
            position = (v.position[0] + direction[0], v.position[1] + direction[1])
            if self.is_valid_position(position):
                neighbors.add(Node(position, direction))
        return neighbors

    def edge_weight(self, u: Node, v: Node) -> int:
        if u.position[0] != u.position[0] and u.position[1] != u.position[1]:
            raise ValueError("Theses nodes are not neighbors.")
        return 1 if u.direction == v.direction else 1001

    def is_valid_position(self, position: tuple[int, int]) -> bool:
        if 1 <= position[0] < len(self.maze) - 1 and 1 <= position[1] < len(self.maze[0]) - 1 and self.maze[position[0]][position[1]] != "#":
            return True
        return False


def main(input_path: Path) -> int:
    graph = Graph.load_from_file(input_path)
    dist = defaultdict(lambda: float("inf"))
    visited = defaultdict(bool)
    priority_queue = []

    source = graph.source_vertex
    dist[source] = 0
    heapq.heappush(priority_queue, (0, source))

    while priority_queue:
        _, u = heapq.heappop(priority_queue)

        if visited[u]:
            continue
        visited[u] = True

        for neighbor in graph.neighbors(u):
            if not visited[neighbor]:
                alt = dist[u] + graph.edge_weight(u, neighbor)
                if alt < dist[neighbor]:
                    dist[neighbor] = alt
                    heapq.heappush(priority_queue, (dist[neighbor], neighbor))

    return min([score for v, score in dist.items() if v.position == graph.end])


if __name__ == "__main__":
    print(f"Score: {main(Path('../data/day_16/input.txt'))}")
