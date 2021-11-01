# BookingAPI

## BD
Par soucis de rapidité il n'y a pas de base de données de créée. Le modèle prévu dans le code est le suivant :
- Id = (Primary Key)
- StartDate = DateTime
- EndDate = DateTime
- CreatedDate = DateTime
- ModifyDate = DateTime

## API
### Cache
Mise en cache côté client afin de ne pas surcharger le serveur d'appel étant donné que l'hôtel est le seul actif beaucoup d'appels sont possibles.

### Architecture
L'architecture mise en place est simpliste. Controller / Service / Repository.
Les contrôles de validations du modèle reçu en entrée du web service se font au niveau du controller. 
Ceci dans le but de ne pas propager aux couches suivantes inutilement afin d'être plus performant.
