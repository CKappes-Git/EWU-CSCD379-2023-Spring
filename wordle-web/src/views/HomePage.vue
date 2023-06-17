<template>
    <v-container style="height: 90vh;">
        <v-row style="justify-content: center; align-items: center; height: 100%; width: 100%; overflow-x: auto;">
            <v-col>
                <RouterLink :to="{ name: 'civilization' }" style="margin: 0; padding: 0;">
                    <v-card class="homeButton" :style="{'background-image': `url(https://images.ctfassets.net/wn7ipiv9ue5v/2rFVmnqp4nDgHmtO3qywHd/4f1139f898494ba7bf8e0b9f8ed1634f/2KGMKT_CIV6_D2C_Hero_Banner_3840x2160__3_.jpg)`}">
                        <p class="cardTitle">Civilization 6</p>
                    </v-card>
                </RouterLink>
            </v-col>
            <v-col>
                <RouterLink :to="{ name: 'home' }" style="margin: 0; padding: 0;">
                    <v-card class="homeButton" :style="{'background-image': `url(https://i.ytimg.com/vi/WNUnz8FUsLc/maxresdefault.jpg)`}">
                        <p class="cardTitle">Coming Soon</p>
                    </v-card>
                </RouterLink>
            </v-col>
        </v-row>
    </v-container>
</template>
<style scoped>
.homeButton{
    height: 500px;
    width: 500px;
    background-size: cover;
    background-position: center;
    text-align: center;
    opacity: .9;
}
.homeButton:hover{
    opacity: 1;
}
.cardTitle{
    justify-content: center;
    align-self: center;
    font-size: 500%;
    height: 70%;
    color: white;
}
</style>