--https://www.youtube.com/watch?v=H4YIKwAQMKk

--Start cluster
minikube start --memory=16384 --cpus=4 --driver docker

--Upload image
minikube image load canary:latest

--Install istio
istioctl install
kubectl get pods -n istio-system

--Run kubectl
kubectl apply -f .\kubernetes\
kubectl get pods -n staging


--Get pods IPs
kubectl get pods -n staging -o wide

--Testing pods (connect and ping)
kubectl exec -it client -n backend -- sh

while true; do curl http://api-test.staging:8080 && echo "" && sleep 1; done