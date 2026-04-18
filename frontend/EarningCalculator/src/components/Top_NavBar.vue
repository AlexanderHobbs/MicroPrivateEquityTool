<script setup>

</script>

<template>
<div class = "parent">
    <div class = "logo"><img src="@/assets/logo.png" alt="logo"></div>
    <div class = "name"><img src="@/assets/name.png" alt="name"></div>
    <div class = "settings"><img src="@/assets/setting.png" alt="settings"></div>
</div>
</template>

<style scoped>
.parent{
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
    margin: 0px 5px;
    background: #ffffff;
    padding: 12px 14px;
    border-radius: 12px;
    border: 1px solid #e5e7eb;
    box-shadow: 0 2px 6px rgba(0,0,0,0.04);
}

.logo img {
   width: 50px;
   height: auto;
   border: 2px solid #f4f6f8;
   border-radius: 5px;
}

.name img {
   width: 100px;
   height: auto;
   border: 2px solid #f4f6f8;
   border-radius: 5px;
}

.settings img { 
   width: 50px;
   height: auto;
   border: 2px solid #f4f6f8;
   border-radius: 5px;
}
</style>    