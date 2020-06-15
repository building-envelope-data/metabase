#!/bin/bash

. ./registerInstitutionsIseAndLbnl.sh
read -p "Press any key to continue ..."
. ./addDatabasesIseAndLbnl.sh
read -p "Press any key to continue ..."
. ./generateComponentIdsAndAddManufacturers.sh
read -p "Press any key to continue ..."
. ./addOpticalDataToIseDatabase.sh
read -p "Press any key to continue ..."
. ./addOpticalDataToLbnlDatabase.sh
read -p "Press any key to continue ..."
. ./getOpticalDataOfGlazingComponent.sh
