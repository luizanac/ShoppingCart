az group create --name microservices --location brazilsouth

az acr create --resource-group microservices --name
microservices-registry --sku Basic

az aks create --resource-group microservices --name 
    microservices-cluster --node-count 1 --enable-addons monitoring --attach-acr microservices-registry

az aks get-credentials --resource-group microservices --name microservices-cluster

kubectl get nodes