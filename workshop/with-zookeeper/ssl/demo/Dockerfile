FROM golang:1.23.0 AS builder
WORKDIR /app
COPY . .
RUN go mod tidy
ENV CGO_ENABLED=1
CMD ["go", "run", "cmd/hello.go"]