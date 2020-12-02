FROM golang:1.13

WORKDIR /app
ENV SRC_DIR=/go/src/github.com/lmolkova/go-app/
ADD . $SRC_DIR
# RUN go get -u go.opencensus.io
# RUN go get -u contrib.go.opencensus.io/exporter/ocagent
RUN cd $SRC_DIR \
  && go mod init local/build \
  && go get -d -v go.opencensus.io@v0.22.3 \
  && go get -d -v contrib.go.opencensus.io/exporter/ocagent@0.7.1 \
  && go build -o goapp \
  && cp goapp /app/
# RUN cd $SRC_DIR; go build -o goapp; cp goapp /app/
EXPOSE 50030
ENTRYPOINT ["./goapp"]