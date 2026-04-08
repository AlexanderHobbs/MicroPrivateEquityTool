<template>
  <div class="jazz-container">
    <div class="text-center mb-12 reveal">
      <h1 class="font-jazz text-5xl font-bold mb-4">Weekly Schedule</h1>
      <div class="w-24 h-1 bg-jazz-gold mx-auto mb-6"></div>
      <p class="text-gray-300 max-w-2xl mx-auto">Join us for unforgettable nights of world-class jazz</p>
    </div>

    <div class="max-w-4xl mx-auto space-y-4 reveal">
      <ScheduleItem 
        v-for="(show, index) in weeklySchedule" 
        :key="show.day"
        :show="show"
        :index="index"
      />
    </div>

    <!-- Reservation CTA -->
    <div class="text-center mt-16 reveal">
      <div class="jazz-card inline-block p-8">
        <h3 class="font-jazz text-2xl font-bold mb-4">Ready for an unforgettable night?</h3>
        <button class="jazz-button text-lg">Make a Reservation</button>
      </div>
    </div>
  </div>
</template>

<script>
import ScheduleItem from '@/components/ScheduleItem.vue'

export default {
  name: 'ScheduleView',
  components: {
    ScheduleItem
  },
  data() {
    return {
      weeklySchedule: [
        { day: 'Monday', artist: 'Blue Note Trio', time: '8:00 PM', genre: 'Classic Jazz' },
        { day: 'Tuesday', artist: 'Sarah Vaughan Memorial', time: '8:00 PM', genre: 'Vocal Jazz' },
        { day: 'Wednesday', artist: 'Modern Jazz Quartet', time: '9:00 PM', genre: 'Cool Jazz' },
        { day: 'Thursday', artist: 'Latin Jazz Experience', time: '8:30 PM', genre: 'Latin Jazz' },
        { day: 'Friday', artist: 'Swing Night Orchestra', time: '10:00 PM', genre: 'Swing' },
        { day: 'Saturday', artist: 'Best of Bebop', time: '9:30 PM', genre: 'Bebop' },
        { day: 'Sunday', artist: 'Sunday Session Jam', time: '7:00 PM', genre: 'Open Mic' }
      ]
    }
  }
}
</script>