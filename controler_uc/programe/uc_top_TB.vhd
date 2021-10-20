--------------------------------------------------------------------------------
-- Company: 
-- Engineer:
--
-- Create Date:   03:56:21 10/20/2021
-- Design Name:   
-- Module Name:   C:/LUCRU/controler_uc/programe/uc_top_TB.vhd
-- Project Name:  controler_uc
-- Target Device:  
-- Tool versions:  
-- Description:   
-- 
-- VHDL Test Bench Created by ISE for module: uc_top
-- 
-- Dependencies:
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
--
-- Notes: 
-- This testbench has been automatically generated using types std_logic and
-- std_logic_vector for the ports of the unit under test.  Xilinx recommends
-- that these types always be used for the top-level I/O of a design in order
-- to guarantee that the testbench will bind correctly to the post-implementation 
-- simulation model.
--------------------------------------------------------------------------------
LIBRARY ieee;
USE ieee.std_logic_1164.ALL;
 
-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--USE ieee.numeric_std.ALL;
 
ENTITY uc_top_TB IS
END uc_top_TB;
 
ARCHITECTURE behavior OF uc_top_TB IS 
 
    -- Component Declaration for the Unit Under Test (UUT)
 
    COMPONENT uc_top
    PORT(
         clk : IN  std_logic;
         btn_reset : IN  std_logic;
         sw : IN  std_logic_vector(7 downto 0);
         btn : IN  std_logic_vector(3 downto 0);
         anod : OUT  std_logic_vector(3 downto 0);
         segment : OUT  std_logic_vector(7 downto 0);
         led : OUT  std_logic_vector(7 downto 0);
         port_intrare1 : IN  std_logic_vector(7 downto 0);
         port_iesire1 : OUT  std_logic_vector(7 downto 0);
         port_in_out1 : INOUT  std_logic_vector(7 downto 0);
         tx : OUT  std_logic;
         rx : IN  std_logic
        );
    END COMPONENT;
    

   --Inputs
   signal clk : std_logic := '0';
   signal btn_reset : std_logic := '0';
   signal sw : std_logic_vector(7 downto 0) := (others => '0');
   signal btn : std_logic_vector(3 downto 0) := (others => '0');
   signal port_intrare1 : std_logic_vector(7 downto 0) := (others => '0');
   signal rx : std_logic := '0';

	--BiDirs
   signal port_in_out1 : std_logic_vector(7 downto 0);

 	--Outputs
   signal anod : std_logic_vector(3 downto 0);
   signal segment : std_logic_vector(7 downto 0);
   signal led : std_logic_vector(7 downto 0);
   signal port_iesire1 : std_logic_vector(7 downto 0);
   signal tx : std_logic;

   -- Clock period definitions
   constant clk_period : time := 10 ns;
 
BEGIN
 
	-- Instantiate the Unit Under Test (UUT)
   uut: uc_top PORT MAP (
          clk => clk,
          btn_reset => btn_reset,
          sw => sw,
          btn => btn,
          anod => anod,
          segment => segment,
          led => led,
          port_intrare1 => port_intrare1,
          port_iesire1 => port_iesire1,
          port_in_out1 => port_in_out1,
          tx => tx,
          rx => rx
        );

   -- Clock process definitions
   clk_process :process
   begin
		clk <= '0';
		wait for clk_period/2;
		clk <= '1';
		wait for clk_period/2;
   end process;
 

   -- Stimulus process
   stim_proc: process
   begin		
      -- hold reset state for 100 ns.
      wait for 100 ns;	

      wait for clk_period*10;

      -- insert stimulus here 

      wait;
   end process;

END;
