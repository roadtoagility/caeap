


`k3d cluster create ecommerce --registry-create ecommerce-registry --port "8088:8088@loadbalancer" --port "18080:8080@loadbalancer" --port "18080:8080@loadbalancer" --servers 1 --agents 2`

curl -X POST -H "Content-Type: application/vnd.schemaregistry.v1+json" \
--data '{"schema": "{\"type\":\"record\",\"name\":\"Payment\",\"namespace\":\"io.confluent.examples.clients.basicavro\",\"fields\":[{\"name\":\"id\",\"type\":\"string\"},{\"name\":\"amount\",\"type\":\"double\"}]}"}' \
http://localhost:8081/subjects/ecommerce.product/versions

curl -X POST -H "Content-Type: application/vnd.schemaregistry.v1+json" \
  --data @EcommerceWebAPI/schemas/ProductCreatedEvent.avsc \
  http://localhost:8081/subjects/ecommerce.product/versions

adriano@morpheus:~/Projects/roadtoagility/dflow-samples/Ecommerce/Ecommerce/proto$ ~/.nuget/packages/google.protobuf.tools/3.21.7/tools/linux_x64/protoc -I=/home/adriano/.nuget/packages/google.protobuf.tools/3.21.7/tools -I=. --csharp_out=generated product-event-created.proto

adriano@morpheus:~/Projects/roadtoagility/dflow-samples/Ecommerce/Ecommerce/proto$ ~/.nuget/packages/google.protobuf.tools/3.21.7/tools/linux_x64/protoc -I=/home/adriano/.nuget/packages/google.protobuf.tools/3.21.7/tools -I=. --csharp_out=generated product-aggregate.proto
