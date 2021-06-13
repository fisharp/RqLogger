#!/usr/bin/env bash

create_assembly_compliant() {
cat > ${1}/Compliant.cs <<EOF
using System;

[assembly: CLSCompliant(false)]
EOF
}

Base='RqLogger'
Group=${1}
Name=${2}
AppTemplate=${3}    # [webapi|classlib|webapp|console|...]
TestTemplate=${4}   # [mstest|nunit|xunit]

[[ ${PWD##*/} != ".scripts" ]] || cd ..
cd Solution

dotnet new ${AppTemplate} -n ${Base}.${Name} -o ${Group}/${Base}.${Name}/Code
dotnet sln add ${Group}/${Base}.${Name}/Code/${Base}.${Name}.csproj
create_assembly_compliant "${Group}/${Base}.${Name}/Code"
printf $"\nProject ${Group}/${Base}.${Name} Done\n\n"

[[ ! -z ${4} ]] || exit 0

printf 'Adding Tests\n'
dotnet new ${TestTemplate} -n ${Base}.${Name}.Tests -o ${Group}/${Base}.${Name}/Tests
dotnet add ${Group}/${Base}.${Name}/Tests/${Base}.${Name}.Tests.csproj reference ${Group}/${Base}.${Name}/Code/${Base}.${Name}.csproj
dotnet sln add ${Group}/${Base}.${Name}/Tests/${Base}.${Name}.Tests.csproj
create_assembly_compliant "${Group}/${Base}.${Name}/Tests"
printf $"\nTest Project ${Group}/${Base}.${Name}.Tests Done\n\n"
