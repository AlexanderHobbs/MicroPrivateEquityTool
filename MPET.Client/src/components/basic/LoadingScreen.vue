<script setup>
import { onMounted, onUnmounted, ref } from 'vue'

defineProps(['iconSrc']);

const canvasRef = ref(null)
const arcRef = ref(null)
let animId = null

const cx = 144, cy = 144, r = 133
const circumference = 2 * Math.PI * r
const arcLength = circumference * 0.25
const duration = 2000
const particles = []

function getTipXY(progress) {
  const angle = -Math.PI / 2 + progress * Math.PI * 2
  return { x: cx + r * Math.cos(angle), y: cy + r * Math.sin(angle) }
}

function spawnDust(x, y, progress) {
  const tangentAngle = -Math.PI / 2 + progress * Math.PI * 2 + Math.PI / 2
  for (let i = 0; i < 3; i++) {
    const spread = (Math.random() - 0.5) * 1.2
    const speed = 0.4 + Math.random() * 1.0
    const angle = tangentAngle + spread + Math.PI + (Math.random() - 0.5) * 0.7
    particles.push({
      x, y,
      vx: Math.cos(angle) * speed,
      vy: Math.sin(angle) * speed,
      life: 1,
      decay: 0.028 + Math.random() * 0.03,
      size: 1.0 + Math.random() * 1.8,
      hue: 38 + Math.floor(Math.random() * 20)
    })
  }
}

function updateParticles(ctx) {
  ctx.clearRect(0, 0, 288, 288)
  for (let i = particles.length - 1; i >= 0; i--) {
    const p = particles[i]
    p.x += p.vx; p.y += p.vy; p.vy += 0.018; p.life -= p.decay
    if (p.life <= 0) { particles.splice(i, 1); continue }
    ctx.beginPath()
    ctx.arc(p.x, p.y, p.size * p.life, 0, Math.PI * 2)
    ctx.fillStyle = `hsla(${p.hue}, 100%, 60%, ${p.life * 0.85})`
    ctx.fill()
  }
}

onMounted(() => {
  const ctx = canvasRef.value.getContext('2d')
  const arc = arcRef.value
  const svg = arc.closest('svg')
  const svgNS = 'http://www.w3.org/2000/svg'

  // Build dynamic gradient
  const defs = document.createElementNS(svgNS, 'defs')
  const grad = document.createElementNS(svgNS, 'linearGradient')
  grad.setAttribute('id', 'arcGrad')
  grad.setAttribute('gradientUnits', 'userSpaceOnUse')

  const stops = [
    { offset: '0%',   color: '#f59e0b', opacity: '0' },
    { offset: '50%',  color: '#f59e0b', opacity: '1' },
    { offset: '100%', color: '#fde68a', opacity: '1' },
  ]
  stops.forEach(({ offset, color, opacity }) => {
    const s = document.createElementNS(svgNS, 'stop')
    s.setAttribute('offset', offset)
    s.setAttribute('stop-color', color)
    s.setAttribute('stop-opacity', opacity)
    grad.appendChild(s)
  })
  defs.appendChild(grad)
  svg.insertBefore(defs, svg.firstChild)

  let startTime = null

  function animate(ts) {
    if (!startTime) startTime = ts
    const elapsed = (ts - startTime) % duration
    const progress = elapsed / duration

    arc.style.strokeDasharray = `${arcLength} ${circumference - arcLength}`
    arc.style.strokeDashoffset = -(progress * circumference)

    const tailAngle = -Math.PI / 2 + progress * Math.PI * 2
    const tipAngle  = tailAngle + Math.PI * 0.5

    grad.setAttribute('x1', cx + r * Math.cos(tailAngle))
    grad.setAttribute('y1', cy + r * Math.sin(tailAngle))
    grad.setAttribute('x2', cx + r * Math.cos(tipAngle))
    grad.setAttribute('y2', cy + r * Math.sin(tipAngle))

    const tipProgress = (progress + 0.25) % 1
    const tip = getTipXY(tipProgress)
    spawnDust(tip.x, tip.y, tipProgress)
    updateParticles(ctx)

    animId = requestAnimationFrame(animate)
  }

  animId = requestAnimationFrame(animate)
})

onUnmounted(() => cancelAnimationFrame(animId))
</script>

<template>
  <transition name="fade-slide" mode="out-in">
    <div class="loader-root">
      <div class="loader-wrap">
        <canvas ref="canvasRef" class="loader-canvas" width="288" height="288"/>
        <svg class="loader-svg" viewBox="0 0 288 288" xmlns="http://www.w3.org/2000/svg">
          <circle ref="arcRef" cx="144" cy="144" r="133"
            fill="none"
            stroke="url(#arcGrad)"
            stroke-width="4"
            stroke-linecap="round"
            stroke-dasharray="0 835.7"
            transform="rotate(-90 144 144)"/>
        </svg>
        <div class="loader-img-wrap">
          <!-- Replace with your logo -->
          <img :src="iconSrc" alt="Logo"/>
        </div>
      </div>
    </div>
  </transition>
</template>

<style scoped>
.fade-slide-enter-active,
.fade-slide-leave-active { transition: all 0.4s ease; }
.fade-slide-enter-from,
.fade-slide-leave-to { opacity: 0; transform: translateY(10px); }

.loader-root {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  min-height: 100vh;
}

.loader-wrap {
  position: relative;
  width: 288px;
  height: 288px;
}

.loader-svg {
  width: 100%;
  height: 100%;
}

.loader-canvas {
  position: absolute;
  top: 0; left: 0;
  width: 100%;
  height: 100%;
  pointer-events: none;
}

.loader-img-wrap {
  position: absolute;
  top: 50%; left: 50%;
  transform: translate(-50%, -50%);
  width: 125px;
  border-radius: 50%;
  overflow: hidden;
  display: flex;
  align-items: center;
  justify-content: center;
  opacity: 90%;
}

.loader-img-wrap img {
  width: 100%;
  height: 100%;
  object-fit: contain;
}
</style>