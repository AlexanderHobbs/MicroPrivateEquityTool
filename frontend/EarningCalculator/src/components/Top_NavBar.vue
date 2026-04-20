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
    padding: 12px 14px;
    border-radius: 12px;

    /* Entrance animation */
    animation: fadeSlideDown 0.6s ease forwards;
}

/* Shared image behavior */
.logo img,
.name img,
.settings img {
   height: auto;
   border: 2px solid #f4f6f8;
   border-radius: 6px;
   transition: all 0.25s ease;
   cursor: pointer;
}

/* Specific sizing */
.logo img { width: 50px; }
.name img { width: 110px; }
.settings img { width: 45px; }

/* Hover interactions */
.logo img:hover {
    transform: scale(1.08) rotate(-2deg);
    box-shadow: 0 6px 14px rgba(0,0,0,0.12);
}

.name img:hover {
    transform: scale(1.05);
    filter: brightness(1.05);
}

.settings img:hover {
    transform: scale(1.08) rotate(-2deg);
    box-shadow: 0 6px 14px rgba(0,0,0,0.12);
}

/* Click feedback */
.logo img:active,
.name img:active,
.settings img:active {
    transform: scale(0.95);
}

/* Keyframes */
@keyframes fadeSlideDown {
    from {
        opacity: 0;
        transform: translateY(-10px);
    }
    to {
        opacity: 1;
        transform: translateY(0);
    }
}
</style>