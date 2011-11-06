using NUnit.Framework;

namespace Senoc.Tests.Routers
{
    [TestFixture]
    public class RouterReceivingClockTests : BaseTest
    {
        [Test]
        [Ignore]
        public void should_redirect_flit_on_buffers()
        {
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void still_tests_missing()
        {
            Assert.Fail();
        }
        /*
         * 
         * =========RESERVE===========
         * foreach channel
         *      if buffer not empty
         *          flit = primeiro flit 
         *          if flit.isHeader?
         *              prepare route data
         *          else
         *              jah estara preparada
         *          end if
         *          
         *          route (route => selection)
         *              com base em:
         *              local_id;
		                flit.src_id;
		                dst_id;
		                this_channel;
         * 
         *          if (reservation_table.isAvailable(o))
		                reservation_table.reserve(i, o);
         *      end if
         *  end foreach
         * 

         * =========FORWARD===========
         * foreach channel
         *      if buffer not empty
         *          flit = primeiro flit 
         *          int o = reservation_table.getOutputPort(channel);
	                if (o != NOT_RESERVED)
         *              if (reserved for this AND accepted)
         *                  send flit
         *                  take flit out from bufer
         *          
         *                  power.Forward()
         *                  
         *                  if (flit is TAIL)
         *                     release reservation
         *      end if
         *  end foreach
         *  
         * else power.stand_by()
         * 
         */ 
//---------------------------------------------------------------------------


    }
}