#!/bin/bash

. ./registerInstitutionsIseAndLbnl.sh
. ./addDatabasesIseAndLbnl.sh
. ./generateComponentIdsAndAddManufacturers.sh
. ./addOpticalDataToIseDatabase.sh
. ./addOpticalDataToLbnlDatabase.sh
. ./getOpticalDataOfGlazingComponent.sh
