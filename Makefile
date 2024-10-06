all: clone-be run

build-push: clone-be build push

in:
	docker exec -it wea_be bash

build:
	@echo "Building WEA_BE image..."
	docker build -t hejsekvojtech/wea_be:latest .

run:
	@echo "Running Docker Compose..."
	docker compose up -d --build

db:
	@echo "Starting DB..."
	docker compose up -d --build db